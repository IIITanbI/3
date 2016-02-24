namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using XmlDesiarilization;

    public class EditItemMeta : BaseEditMeta
    {
        public Window EditWindow { get; set; }
        public XmlBaseType ObjectToEdit { get; set; }
        private IWpfControlFiller _filler;

        public EditItemMeta(Button editButton, XmlBaseType obj)
            : base(editButton)
        {
            ObjectToEdit = obj;
        }

        protected override void EditValueButton_Click(object sender, RoutedEventArgs e)
        {
            var itemXmlType = ReflectionManager.GetXmlType(ObjectToEdit.GetType());

            EditWindow = new Window();
            EditWindow.Title = $"Editing Item with type: {itemXmlType.XType.Name}";

            var scrollViewer = new ScrollViewer();
            EditWindow.Content = scrollViewer;
            var rootStackPanel = new StackPanel();
            scrollViewer.Content = rootStackPanel;

            var buttonsWrapPanel = new WrapPanel();
            rootStackPanel.Children.Add(buttonsWrapPanel);

            SaveButton = new Button { Content = "Save" };
            buttonsWrapPanel.Children.Add(SaveButton);
            SaveButton.Click += SaveEditValueButton_Click;
            CancelButton = new Button { Content = "Cancel" };
            buttonsWrapPanel.Children.Add(CancelButton);
            CancelButton.Click += CancelEditValueButton_Click;

            _filler = ReflectionManager.GetControlFillerForType(itemXmlType).GetFiller();
            _filler.FillEditMode(rootStackPanel, ObjectToEdit);

            EditWindow.ShowDialog();

            e.Handled = true;
        }

        protected override void SaveEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            CreatedValue = _filler.GetValue();
            ParentProperty.SetValue(ParentObject, CreatedValue);

            EditWindow.Close();
            e.Handled = true;
        }

        protected override void CancelEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow.Close();
            e.Handled = true;
        }
    }
}
