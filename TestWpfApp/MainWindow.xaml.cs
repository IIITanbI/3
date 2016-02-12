namespace TestWpfApp
{
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
    using QA.TestLibs.Wpf.XmlTypeControl;
    using QA.TestLibs;
    using QA.TestLibs.XmlDesiarilization;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ReflectionManager.LoadAssemblies();

            InitializeComponent();

            foreach (var xmlType in ReflectionManager.GetXmlTypes())
            {
                XmlTypesListBox.Items.Add(new ListBoxItem() { Content = xmlType });
            }
        }

        private void XmlTypesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var selectedItem = (ListBoxItem)listBox.SelectedItem;

            XmlTypeStackPanel.Children.Clear();
            XmlTypeStackPanel.Children.Add(new XmlTypeUserControl((XmlType)selectedItem.Content));
        }
    }
}
