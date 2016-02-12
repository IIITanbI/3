namespace QA.TestLibs.Wpf.XmlTypeControl
{
    using QA.TestLibs.XmlDesiarilization;
    using System;
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


    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class XmlTypeUserControl : UserControl
    {
        private XmlType _xmlType;

        public XmlTypeUserControl(XmlType type)
        {
            _xmlType = type;

            InitializeComponent();

            XmlTypeNameLabel.Content = $"Type: {_xmlType.XType.Name}";
            XmlTypeDescriptionLabel.Content = $"Description: {_xmlType.Description}";

            foreach (var xmlProperty in _xmlType.XmlProperties)
            {
                XmlPropertiesListBox.Items.Add(new ListBoxItem { Content = xmlProperty });
            }
        }
    }
}
