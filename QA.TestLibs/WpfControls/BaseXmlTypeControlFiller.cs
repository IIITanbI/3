namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;
    using System.Windows.Controls;
    using System.Collections;
    using CustomFillers;
    using System.Windows;
    using System.Windows.Media;
    using ExtensionMethods;

    [WpfControlFiller(typeof(XmlBaseType))]
    public class BaseXmlTypeControlFiller : IWpfControlFiller
    {
        protected List<BaseValueFiller> _valueFillers = new List<BaseValueFiller>
        {
            new ByteFiller(),
            new IntFiller(),
            new DoubleFiller(),
            new StringFiller(),
            new EnumFiller(),
            new DateTimeFiller(),
            new TimeSpanFiller(),
            new BoolFiller(),
            new XElementFiller()
        };

        public Func<object> GetValue { get; protected set; }
        public Action<object> SetValue { get; protected set; }

        public void FillCreateMode(Panel parentPanel, Type type1)
        {
            var xmlType = ReflectionManager.GetXmlType(type1);
            var obj = Activator.CreateInstance(type1);

            var propertiesExpander = new Expander { Header = "Properties" };
            parentPanel.Children.Add(propertiesExpander);
            var propertiesStackPanel = new StackPanel();
            propertiesExpander.Content = propertiesStackPanel;

            var itemsExpander = new Expander { Header = "Items" };
            parentPanel.Children.Add(itemsExpander);
            var itemsStackPanel = new StackPanel();
            itemsExpander.Content = itemsStackPanel;

            var collectionExpander = new Expander { Header = "Collections" };
            parentPanel.Children.Add(collectionExpander);
            var collectionsStackPanel = new StackPanel();
            collectionExpander.Content = collectionsStackPanel;

            foreach (var xmlProperty in xmlType.XmlProperties)
            {
                var type = xmlProperty.PropertyType;
                var propertyValue = xmlProperty.GetValue(obj);

                var xmlPropertyGroupBox = new GroupBox();
                var xmlPropertyHeaderWrapPanel = new WrapPanel();
                xmlPropertyGroupBox.Header = xmlPropertyHeaderWrapPanel;
                xmlPropertyHeaderWrapPanel.Children.Add(new Label { Content = xmlProperty.Info.Name });

                var editXmlPropertyButton = new Button { Content = "Edit" };
                xmlPropertyHeaderWrapPanel.Children.Add(editXmlPropertyButton);

                var xmlPropertyToolTip = new ToolTip { Content = xmlProperty.Description };
                xmlPropertyGroupBox.ToolTip = xmlPropertyToolTip;

                if (type.IsGenericType)
                {
                    var meta = new EditCollectionMeta(editXmlPropertyButton, propertyValue);
                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;
                    meta.ValueFillers = GetValueFillers();
                    meta.IsAssignableTypesAllowed = xmlProperty.IsAssignableTypesAllowed;

                    collectionsStackPanel.Children.Add(xmlPropertyGroupBox);
                    var childsStackPanel = new StackPanel();
                    xmlPropertyGroupBox.Content = childsStackPanel;

                    var children = propertyValue.GetChilds();
                    childsStackPanel.Children.Add(new Label { Content = $"Count: {children.Count}" });

                    var counter = 1;
                    foreach (var child in children)
                    {
                        var childType = child.GetType();

                        var childGroupBox = new GroupBox { Header = $"Item #{counter++}" };
                        childsStackPanel.Children.Add(childGroupBox);

                        if (typeof(XmlBaseType).IsAssignableFrom(childType))
                        {
                            childGroupBox.Content = GenerateExpanderForXmlTypeObjectEdit(childType, child, null);
                        }
                        else
                        {
                            childGroupBox.Content = GenerateStackPanelForValueEdit(childType, child, null);
                        }
                    }

                }
                else if (typeof(XmlBaseType).IsAssignableFrom(type))
                {
                    var meta = new EditItemMeta(editXmlPropertyButton, (XmlBaseType)propertyValue);
                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;

                    itemsStackPanel.Children.Add(xmlPropertyGroupBox);
                    var itemExpander = GenerateExpanderForXmlTypeObjectEdit(type, propertyValue, meta);
                    xmlPropertyGroupBox.Content = itemExpander;
                }
                else
                {
                    var saveXmlPropertyButton = new Button { Content = "Save", Visibility = Visibility.Hidden };
                    xmlPropertyHeaderWrapPanel.Children.Add(saveXmlPropertyButton);
                    var cancelXmlPropertyButton = new Button { Content = "Cancel", Visibility = Visibility.Hidden };
                    xmlPropertyHeaderWrapPanel.Children.Add(cancelXmlPropertyButton);

                    var meta = new EditValueMeta(editXmlPropertyButton);

                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;
                    meta.SaveButton = saveXmlPropertyButton;
                    meta.CancelButton = cancelXmlPropertyButton;

                    propertiesStackPanel.Children.Add(xmlPropertyGroupBox);
                    var propertyStackPanel = GenerateStackPanelForValueEdit(type, propertyValue, meta);
                    meta.Container = propertyStackPanel;
                    propertyStackPanel.IsEnabled = false;
                    xmlPropertyGroupBox.Content = propertyStackPanel;
                }
            }

            GetValue = () => obj;
        }

        public void FillEditMode(Panel parentPanel, object obj)
        {
            var xmlType = ReflectionManager.GetXmlType(obj.GetType());

            var propertiesExpander = new Expander { Header = "Properties" };
            parentPanel.Children.Add(propertiesExpander);
            var propertiesStackPanel = new StackPanel();
            propertiesExpander.Content = propertiesStackPanel;

            var itemsExpander = new Expander { Header = "Items" };
            parentPanel.Children.Add(itemsExpander);
            var itemsStackPanel = new StackPanel();
            itemsExpander.Content = itemsStackPanel;

            var collectionExpander = new Expander { Header = "Collections" };
            parentPanel.Children.Add(collectionExpander);
            var collectionsStackPanel = new StackPanel();
            collectionExpander.Content = collectionsStackPanel;

            foreach (var xmlProperty in xmlType.XmlProperties)
            {
                var type = xmlProperty.PropertyType;
                var propertyValue = xmlProperty.GetValue(obj);

                var xmlPropertyGroupBox = new GroupBox();
                var xmlPropertyHeaderWrapPanel = new WrapPanel();
                xmlPropertyGroupBox.Header = xmlPropertyHeaderWrapPanel;
                xmlPropertyHeaderWrapPanel.Children.Add(new Label { Content = xmlProperty.Info.Name });

                var editXmlPropertyButton = new Button { Content = "Edit" };
                xmlPropertyHeaderWrapPanel.Children.Add(editXmlPropertyButton);

                var xmlPropertyToolTip = new ToolTip { Content = xmlProperty.Description };
                xmlPropertyGroupBox.ToolTip = xmlPropertyToolTip;

                if (type.IsGenericType)
                {
                    var meta = new EditCollectionMeta(editXmlPropertyButton, propertyValue);
                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;
                    meta.ValueFillers = GetValueFillers();
                    meta.IsAssignableTypesAllowed = xmlProperty.IsAssignableTypesAllowed;

                    collectionsStackPanel.Children.Add(xmlPropertyGroupBox);
                    var childsStackPanel = new StackPanel();
                    xmlPropertyGroupBox.Content = childsStackPanel;

                    var children = propertyValue.GetChilds();
                    childsStackPanel.Children.Add(new Label { Content = $"Count: {children.Count}" });

                    var counter = 1;
                    foreach (var child in children)
                    {
                        var childType = child.GetType();

                        var childGroupBox = new GroupBox { Header = $"Item #{counter++}" };
                        childsStackPanel.Children.Add(childGroupBox);

                        if (typeof(XmlBaseType).IsAssignableFrom(childType))
                        {
                            childGroupBox.Content = GenerateExpanderForXmlTypeObjectEdit(childType, child, null);
                        }
                        else
                        {
                            childGroupBox.Content = GenerateStackPanelForValueEdit(childType, child, null);
                        }
                    }

                }
                else if (typeof(XmlBaseType).IsAssignableFrom(type))
                {
                    var meta = new EditItemMeta(editXmlPropertyButton, (XmlBaseType)propertyValue);
                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;

                    itemsStackPanel.Children.Add(xmlPropertyGroupBox);
                    var itemExpander = GenerateExpanderForXmlTypeObjectEdit(type, propertyValue, meta);
                    xmlPropertyGroupBox.Content = itemExpander;
                }
                else
                {
                    var saveXmlPropertyButton = new Button { Content = "Save", Visibility = Visibility.Hidden };
                    xmlPropertyHeaderWrapPanel.Children.Add(saveXmlPropertyButton);
                    var cancelXmlPropertyButton = new Button { Content = "Cancel", Visibility = Visibility.Hidden };
                    xmlPropertyHeaderWrapPanel.Children.Add(cancelXmlPropertyButton);

                    var meta = new EditValueMeta(editXmlPropertyButton);

                    meta.ParentProperty = xmlProperty;
                    meta.ParentObject = obj;
                    meta.SaveButton = saveXmlPropertyButton;
                    meta.CancelButton = cancelXmlPropertyButton;

                    propertiesStackPanel.Children.Add(xmlPropertyGroupBox);
                    var propertyStackPanel = GenerateStackPanelForValueEdit(type, propertyValue, meta);
                    meta.Container = propertyStackPanel;
                    propertyStackPanel.IsEnabled = false;
                    xmlPropertyGroupBox.Content = propertyStackPanel;
                }
            }

            GetValue = () => obj;
        }

        public virtual Expander GenerateExpanderForXmlTypeObjectEdit(Type type, object propertyValue, EditItemMeta meta)
        {
            var itemXmlType = ReflectionManager.GetXmlType(type);

            var itemExpander = new Expander { Header = itemXmlType.XType.Name };
            var itemStackPanel = new StackPanel();
            itemExpander.Content = itemStackPanel;

            var filler = ReflectionManager.GetControlFillerForType(itemXmlType).GetFiller();
            filler.FillEditMode(itemStackPanel, (XmlBaseType)propertyValue);

            if (meta != null)
            {
                meta.ControlGetValue = filler.GetValue;
                meta.ControlSetValue = filler.SetValue;
            }

            return itemExpander;
        }
        public virtual StackPanel GenerateStackPanelForValueEdit(Type type, object propertyValue, EditValueMeta meta)
        {
            var propertyStackPanel = new StackPanel();

            var valueFiller = GetValueFillers().First(vf => vf.IsMatch(type));
            valueFiller.FillEditMode(propertyStackPanel, propertyValue);

            if (meta != null)
            {
                meta.ControlSetValue = valueFiller.SetValue;
                meta.ControlGetValue = valueFiller.GetValue;
            }

            return propertyStackPanel;
        }

        public void FillInfoMode(Panel parentPanel, object obj)
        {
            var xmlType = ReflectionManager.GetXmlType(obj.GetType());

            var propertiesExpander = new Expander { Header = "Properties" };
            parentPanel.Children.Add(propertiesExpander);
            var propertiesStackPanel = new StackPanel();
            propertiesExpander.Content = propertiesStackPanel;

            var itemsExpander = new Expander { Header = "Items" };
            parentPanel.Children.Add(itemsExpander);
            var itemsStackPanel = new StackPanel();
            itemsExpander.Content = itemsStackPanel;

            var collectionExpander = new Expander { Header = "Collections" };
            parentPanel.Children.Add(collectionExpander);
            var collectionsStackPanel = new StackPanel();
            collectionExpander.Content = collectionsStackPanel;

            var hasItem = false;
            var hasProperty = false;
            var hasCollection = false;

            foreach (var xmlProperty in xmlType.XmlProperties)
            {
                var type = xmlProperty.PropertyType;
                var propertyValue = xmlProperty.GetValue(obj);

                var xmlPropertyGroupBox = new GroupBox { Header = xmlProperty.Info.Name };
                var xmlPropertyToolTip = new ToolTip { Content = xmlProperty.Description };
                xmlPropertyGroupBox.ToolTip = xmlPropertyToolTip;

                if (type.IsGenericType)
                {
                    hasCollection = true;

                    collectionsStackPanel.Children.Add(xmlPropertyGroupBox);
                    var childsStackPanel = new StackPanel();
                    xmlPropertyGroupBox.Content = childsStackPanel;

                    var children = propertyValue.GetChilds();
                    childsStackPanel.Children.Add(new Label { Content = $"Count: {children.Count}" });

                    var counter = 1;
                    foreach (var child in children)
                    {
                        var childType = child.GetType();

                        var childGroupBox = new GroupBox { Header = $"Item #{counter++}" };
                        childsStackPanel.Children.Add(childGroupBox);

                        if (typeof(XmlBaseType).IsAssignableFrom(childType))
                            childGroupBox.Content = GenerateExpanderForXmlTypeObjectInfo(childType, child);
                        else
                            childGroupBox.Content = GenerateStackPanelForValueInfo(childType, child);
                    }

                }
                else if (typeof(XmlBaseType).IsAssignableFrom(type))
                {
                    hasItem = true;

                    itemsStackPanel.Children.Add(xmlPropertyGroupBox);
                    xmlPropertyGroupBox.Content = GenerateExpanderForXmlTypeObjectInfo(type, propertyValue);
                }
                else
                {
                    hasProperty = true;

                    propertiesStackPanel.Children.Add(xmlPropertyGroupBox);
                    xmlPropertyGroupBox.Content = GenerateStackPanelForValueInfo(type, propertyValue);
                }
            }

            if (!hasProperty)
                parentPanel.Children.Remove(propertiesExpander);
            if (!hasItem)
                parentPanel.Children.Remove(itemsExpander);
            if (!hasCollection)
                parentPanel.Children.Remove(collectionExpander);
        }
        public virtual Expander GenerateExpanderForXmlTypeObjectInfo(Type type, object propertyValue)
        {
            var itemXmlType = ReflectionManager.GetXmlType(type);

            var itemExpander = new Expander { Header = itemXmlType.XType.Name };
            var itemStackPanel = new StackPanel();
            itemExpander.Content = itemStackPanel;

            var filler = ReflectionManager.GetControlFillerForType(itemXmlType).GetFiller();
            filler.FillInfoMode(itemStackPanel, (XmlBaseType)propertyValue);
            return itemExpander;
        }
        public virtual StackPanel GenerateStackPanelForValueInfo(Type type, object propertyValue)
        {
            var propertyStackPanel = new StackPanel();

            var valueFiller = GetValueFillers().First(vf => vf.IsMatch(type));
            valueFiller.FillInfoMode(propertyStackPanel, propertyValue);
            return propertyStackPanel;
        }

        public List<BaseValueFiller> GetValueFillers()
        {
            return _valueFillers;
        }

        public bool IsMatch(Type type)
        {
            return (typeof(XmlBaseType).IsAssignableFrom(type));
        }
    }
}
