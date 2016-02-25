namespace QA.TestLibs.WpfControls
{
    using CustomFillers;
    using ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using XmlDesiarilization;

    public class EditCollectionMeta : BaseEditMeta
    {
        public Window EditWindow { get; set; }
        public object ObjectToEdit { get; set; }
        public bool IsAssignableTypesAllowed { get; set; }

        public List<BaseEditMeta> ChildMetas { get; set; } = new List<BaseEditMeta>();
        public List<BaseValueFiller> ValueFillers = new List<BaseValueFiller>();

        public EditCollectionMeta(Button editButton, object obj)
            : base(editButton)
        {
            ObjectToEdit = obj;
        }

        protected override void EditValueButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow = new Window();

            var scrollViewer = new ScrollViewer();
            EditWindow.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var buttonsWrapPanel = new WrapPanel();
            rootStackPanel.Children.Add(buttonsWrapPanel);

            var newButton = new Button { Content = "New" };
            buttonsWrapPanel.Children.Add(newButton);
            newButton.Click += NewButton_Click;


            var childrenStackPanel = new StackPanel();
            rootStackPanel.Children.Add(childrenStackPanel);


            var meta = new Meta();
            meta.Parent = childrenStackPanel;
            newButton.Tag = meta;


            FillChildren(childrenStackPanel);

            EditWindow.ShowDialog();

            e.Handled = true;
        }

        private void FillChildren(StackPanel rootStackPanel)
        {
            var children = ObjectToEdit.GetChilds();
            rootStackPanel.Children.Add(new Label { Content = $"Count: {children.Count}" });

            var counter = 1;
            foreach (var child in children)
            {
                var childType = child.GetType();

                var childGroupBox = new GroupBox();

                var childGroupBoxHeaderWrapPanel = new WrapPanel();
                childGroupBox.Header = childGroupBoxHeaderWrapPanel;
                childGroupBoxHeaderWrapPanel.Children.Add(new Label { Content = $"Item #{counter++}" });
                var removeButton = new Button { Content = "Remove" };
                childGroupBoxHeaderWrapPanel.Children.Add(removeButton);
                removeButton.Click += RemoveButton_Click;
                var meta = new Meta();
                meta.Container = childGroupBox;
                meta.Parent = rootStackPanel;
                meta.Value = child;
                removeButton.Tag = meta;

                rootStackPanel.Children.Add(childGroupBox);

                if (typeof(XmlBaseType).IsAssignableFrom(childType))
                {
                    var itemXmlType = ReflectionManager.GetXmlType(childType);

                    var itemExpander = new Expander { Header = itemXmlType.XType.Name };
                    var itemStackPanel = new StackPanel();
                    itemExpander.Content = itemStackPanel;

                    var filler = ReflectionManager.GetControlFillerForType(itemXmlType).GetFiller();
                    filler.FillEditMode(itemStackPanel, (XmlBaseType)child);

                    childGroupBox.Content = itemExpander;
                }
                else
                {
                    var propertyStackPanel = new StackPanel();

                    var valueFiller = ValueFillers.First(vf => vf.IsMatch(childType));
                    valueFiller.FillEditMode(propertyStackPanel, child);

                    childGroupBox.Content = propertyStackPanel;
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var meta = (Meta)((Button)sender).Tag;

            meta.Parent.Children.Remove(meta.Container);

            var type = ObjectToEdit.GetType();
            var childObjType = type.GetGenericArguments().Last();

            if (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(LinkedList<>))
            {
                var add = type.GetMethod("Remove", new[] { childObjType });
                add.Invoke(ObjectToEdit, new object[] { meta.Value });
            }
            else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var childXmlType = ReflectionManager.GetXmlType(childObjType);

                var add = type.GetMethod("Remove", new[] { childXmlType.KeyProperty.PropertyType, childObjType });
                var key = childXmlType.KeyProperty.GetValue(meta.Value);
                add.Invoke(ObjectToEdit, new object[] { key });
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var meta = (Meta)((Button)sender).Tag;
            var type = ObjectToEdit.GetType();
            var childObjType = type.GetGenericArguments().Last();

            var createWindow = new Window();
            meta.NewWindow = createWindow;
            var contentScroller = new ScrollViewer();
            createWindow.Content = contentScroller;
            var contentStackPanel = new StackPanel();
            contentScroller.Content = contentStackPanel;

            var buttonsWrapPanel = new WrapPanel();
            contentStackPanel.Children.Add(buttonsWrapPanel);
            var createButton = new Button { Content = "Create" };
            buttonsWrapPanel.Children.Add(createButton);
            var cancelButton = new Button { Content = "Cancel" };
            buttonsWrapPanel.Children.Add(cancelButton);

            cancelButton.Click += CancelEditValueButton_Click;
            cancelButton.Tag = meta;
            createButton.Click += SaveEditValueButton_Click;
            createButton.Tag = meta;

            if (typeof(XmlBaseType).IsAssignableFrom(childObjType))
            {

                if (IsAssignableTypesAllowed)
                {
                    var typesComboBox = new ComboBox();
                    contentStackPanel.Children.Add(typesComboBox);

                    var meta1 = new Meta();
                    meta1.Parent = contentStackPanel;
                    meta1.ChildMeta = meta;
                    typesComboBox.Tag = meta1;

                    var assignableTypes = ReflectionManager.GetAssignableTypes(childObjType);
                    foreach (var at in assignableTypes)
                    {
                        typesComboBox.Items.Add(at);
                    }
                    typesComboBox.SelectedIndex = 0;
                    typesComboBox.SelectionChanged += TypesComboBox_SelectionChanged;
                    childObjType = assignableTypes.First().XType;
                }

                var filler = ReflectionManager.GetControlFillerForType(ReflectionManager.GetXmlType(childObjType)).GetFiller();
                filler.FillCreateMode(contentStackPanel, childObjType);
                meta.Filler = filler;
            }
            else
            {
                var valueFiller = ValueFillers.First(vf => vf.IsMatch(childObjType));
                valueFiller.FillCreateMode(contentStackPanel, childObjType);
                meta.Filler = valueFiller;
            }

            createWindow.ShowDialog();

            e.Handled = true;
        }

        private void TypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var meta = (Meta)((ComboBox)sender).Tag;
            var childObjType = ((XmlType)((ComboBox)sender).SelectedItem).XType;
            var filler = ReflectionManager.GetControlFillerForType(ReflectionManager.GetXmlType(childObjType)).GetFiller();

            meta.Parent.Children.RemoveRange(2, meta.Parent.Children.Count - 2);
            filler.FillCreateMode(meta.Parent, childObjType);
            meta.ChildMeta.Filler = filler;

            e.Handled = true;
        }

        protected override void SaveEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            var meta = (Meta)((Button)sender).Tag;

            var newObj = meta.Filler.GetValue();
            var type = ObjectToEdit.GetType();
            var childObjType = type.GetGenericArguments().Last();

            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var add = type.GetMethod("Add", new[] { childObjType });

                add.Invoke(ObjectToEdit, new object[] { newObj });
            }
            else if (type.GetGenericTypeDefinition() == typeof(LinkedList<>))
            {
                var add = type.GetMethod("AddLast", new[] { childObjType });

                add.Invoke(ObjectToEdit, new object[] { newObj });
            }
            else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var childXmlType = ReflectionManager.GetXmlType(childObjType);

                var add = type.GetMethod("Add", new[] { childXmlType.KeyProperty.PropertyType, childObjType });

                var key = childXmlType.KeyProperty.GetValue(newObj);
                add.Invoke(ObjectToEdit, new object[] { key, newObj });
            }

            meta.NewWindow.Close();
            meta.Parent.Children.Clear();
            FillChildren(meta.Parent);

            e.Handled = true;
        }
        protected override void CancelEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            var meta = (Meta)((Button)sender).Tag;
            meta.NewWindow.Close();
            e.Handled = true;
        }

        private class Meta
        {
            public Meta ChildMeta { get; set; }
            public Window NewWindow { get; set; }
            public StackPanel Parent { get; set; }
            public UIElement Container { get; set; }
            public object Value { get; set; }
            public Type ChildType { get; set; }
            public IWpfControlFiller Filler { get; set; }
        }
    }
}
