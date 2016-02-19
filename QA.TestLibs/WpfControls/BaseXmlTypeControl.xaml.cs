namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using XmlDesiarilization;

    [WpfTypeControl("XmlBaseType")]
    public partial class BaseXmlTypeControl : UserControl, IWpfTypeControl
    {
        public BaseXmlTypeControl()
        {
            InitializeComponent();
        }

        public void GenerateEditMode(XmlBaseType obj)
        {
            throw new NotImplementedException();
        }

        public void GenerateInfoMode(XmlBaseType obj)
        {
            var xmlType = ReflectionManager.GetXmlType(obj.GetType());
            foreach (var xmlProperty in xmlType.XmlProperties)
            {
                GenerateInfoForProperty(RootPanel, xmlProperty, obj);
            }
        }

        public void GenerateInfoForProperty(Panel parent, XmlProperty xmlProperty, XmlBaseType obj)
        {
            if (xmlProperty.PropertyType.IsGenericType)
            {
                var propertyExpander = new Expander();
                parent.Children.Add(propertyExpander);
                var propertyExpamderHeaderStackPanel = new StackPanel();
                propertyExpander.Header = propertyExpamderHeaderStackPanel;
                var nameLabel = new Label { Content = "Name: " + xmlProperty.Info.Name };
                propertyExpamderHeaderStackPanel.Children.Add(nameLabel);
                var descriptionLabel = new Label { Content = $"Description: {xmlProperty.Description}" };
                propertyExpamderHeaderStackPanel.Children.Add(descriptionLabel);

                var childListBox = new ListBox();
                propertyExpander.Content = childListBox;

                var childType = xmlProperty.PropertyType.GetGenericArguments().Last();
                var childObj = xmlProperty.GetValue(obj);
                var childObjs = new List<object>();

                if (childType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    foreach (var cVal in ((IDictionary)childObj).Values) childObjs.Add(cVal);
                else foreach (var cVal in (IEnumerable)childObj) childObjs.Add(cVal);

                if (typeof(XmlBaseType).IsAssignableFrom(childType))
                {
                    foreach (var cObj in childObjs)
                    {
                        var xmlType = ReflectionManager.GetXmlType(childType);
                        var controlType = ReflectionManager.GetControlForType(xmlType);
                        var controlObj = (IWpfTypeControl)Activator.CreateInstance(controlType);
                        controlObj.GenerateInfoMode((XmlBaseType)xmlProperty.GetValue(cObj));
                        childListBox.Items.Add(controlObj);
                    }
                }
                else
                {
                    foreach (var cObj in childObjs)
                    {
                        var valueLabel = new Label { Content = $"Value: {cObj}" };
                        childListBox.Items.Add(valueLabel);
                    }
                }
            }
            else if (typeof(XmlBaseType).IsAssignableFrom(xmlProperty.PropertyType))
            {
                var propertyExpander = new Expander();
                parent.Children.Add(propertyExpander);
                var propertyExpamderHeaderStackPanel = new StackPanel();
                propertyExpander.Header = propertyExpamderHeaderStackPanel;
                var nameLabel = new Label { Content = "Name: " + xmlProperty.Info.Name };
                propertyExpamderHeaderStackPanel.Children.Add(nameLabel);
                var descriptionLabel = new Label { Content = $"Description: {xmlProperty.Description}" };
                propertyExpamderHeaderStackPanel.Children.Add(descriptionLabel);

                var xmlType = ReflectionManager.GetXmlType(xmlProperty.PropertyType);
                var controlType = ReflectionManager.GetControlForType(xmlType);
                var controlObj = (IWpfTypeControl)Activator.CreateInstance(controlType);
                controlObj.GenerateInfoMode((XmlBaseType)xmlProperty.GetValue(obj));
                propertyExpander.Content = controlObj;
            }
            else
            {
                var propertyGroupBox = new GroupBox { Header = xmlProperty.Info.Name };
                parent.Children.Add(propertyGroupBox);
                var propertyGroupStackPanel = new StackPanel();
                propertyGroupBox.Content = propertyGroupStackPanel;
                var descriptionLabel = new Label { Content = $"Description: {xmlProperty.Description}" };
                propertyGroupStackPanel.Children.Add(descriptionLabel);
                var valueLabel = new Label { Content = $"Value: {xmlProperty.GetValue(obj)}" };
                propertyGroupStackPanel.Children.Add(valueLabel);
            }
        }
    }
}
