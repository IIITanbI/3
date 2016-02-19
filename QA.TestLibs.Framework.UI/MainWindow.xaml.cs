namespace QA.TestLibs.Framework.UI
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
    using XmlDesiarilization;
    using Framework;
    using System.Xml.Linq;

    public partial class MainWindow : Window
    {
        private TreeViewItem _rootItem;

        public MainWindow()
        {
            ReflectionManager.LoadAssemblies();
            InitializeComponent();

            _rootItem = new TreeViewItem { Header = "Projects" };
            FTree.Items.Add(_rootItem);

            LoadProject("config.xml");
        }

        private void FMenuProjectOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pathToProjectXml = ofd.FileName;
                LoadProject(pathToProjectXml);
            }
        }

        public void LoadProject(string pathToXml)
        {
            var projectDoc = XDocument.Load(pathToXml);

            try
            {
                var project = XmlParser.Parse<TestProject>(projectDoc.Elements().First());
                var projectTreeItem = new TreeViewItem { Tag = project, Header = $"{project.ItemType}: {project.Name}" };
                projectTreeItem.Items.Add("*");
                projectTreeItem.Expanded += FTreeItem_Expanded;
                projectTreeItem.Selected += FTreeItem_Selected;

                _rootItem.Items.Add(projectTreeItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Following error occurred during opening:", MessageBoxButton.OK);
            }
        }
        private void FTreeItem_Expanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)sender;
            if (treeViewItem.Items.Contains("*"))
            {
                var testItemBase = (TestItem)treeViewItem.Tag;
                if (testItemBase.ItemType != TestItemType.Test)
                {
                    var suite = (TestSuite)treeViewItem.Tag;
                    treeViewItem.Items.Clear();

                    foreach (var testItem in suite.TestItems)
                    {
                        var testItemTreeItem = new TreeViewItem { Tag = testItem, Header = $"{testItem.ItemType}: {testItem.Name}" };
                        if (testItem.ItemType != TestItemType.Test)
                        {
                            testItemTreeItem.Items.Add("*");
                            testItemTreeItem.Expanded += FTreeItem_Expanded;
                        }
                        testItemTreeItem.Selected += FTreeItem_Selected;
                        testItemTreeItem.FontWeight = FontWeights.Normal;
                        treeViewItem.Items.Add(testItemTreeItem);
                    }
                }
            }
            e.Handled = true;
        }

        private TestItem _curTestItem = null;
        private TreeViewItem _curTreeItem = null;
        private void FTreeItem_Selected(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)sender;

            if (_curTreeItem == null || _curTreeItem != treeViewItem)
            {
                if(_curTreeItem != null)
                    _curTreeItem.FontWeight = FontWeights.Normal;
                _curTreeItem = treeViewItem;
                _curTreeItem.FontWeight = FontWeights.Bold;
            }

            if (typeof(TestItem).IsAssignableFrom(treeViewItem.Tag.GetType()))
            {
                var testItem = (TestItem)treeViewItem.Tag;
                if (_curTestItem == null || _curTestItem != testItem)
                {
                    _curTestItem = testItem;
                    FillTab(_curTestItem);
                }
            }
            e.Handled = true;
        }

        public void FillTab(TestItem testItem)
        {
            var selectedTab = (TabItem)FTabs.SelectedItem;

            switch (((string)selectedTab.Header))
            {
                case "Info":
                    FillInfoTab(testItem);
                    break;
                case "Managing":
                    FillManagingTab(testItem);
                    break;
                case "Logs":
                    FillLogsTab(testItem);
                    break;
                case "Report":
                    FillReportTab(testItem);
                    break;
                case "XmlView":
                    FillXmlViewTab(testItem);
                    break;
                default:
                    break;
            }
        }
        public void FillInfoTab(TestItem testItem)
        {
            FInfoPanel.Children.Clear();

            var infoControl = new TestItemControl();
            infoControl.GenerateInfoMode(testItem);

            FInfoPanel.Children.Add(infoControl);
        }
        public void FillManagingTab(TestItem testItem)
        {

        }
        public void FillLogsTab(TestItem testItem)
        {

        }
        public void FillReportTab(TestItem testItem)
        {

        }
        public void FillXmlViewTab(TestItem testItem)
        {

        }

    }
}
