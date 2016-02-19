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

            //Show TestItem name
            var nameLabel = new Label { Content = $"{testItem.ItemType}: {testItem.Name}" };
            FInfoPanel.Children.Add(nameLabel);

            //Show test item description
            var descriptionGroupBox = new GroupBox { Header = "Description:" };
            var descriptionLabel = new Label { Content = testItem.Description };
            descriptionGroupBox.Content = descriptionLabel;
            FInfoPanel.Children.Add(descriptionGroupBox);

            //Show TestItem context
            if (testItem.Context.CommandManagersItems.Count > 0 || testItem.Context.TestContextItems.Count > 0)
            {
                var contextExpander = new Expander { Header = "Context:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
                FInfoPanel.Children.Add(contextExpander);
                var contextStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                contextExpander.Content = contextStackPanel;

                if (testItem.Context.TestContextItems.Count > 0)
                {
                    var contextItemsExpander = new Expander { Header = "ContextItems:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
                    contextStackPanel.Children.Add(contextItemsExpander);
                    var contextItemsStackPanel = new StackPanel { Margin = new Thickness(5) };
                    contextItemsExpander.Content = contextItemsStackPanel;

                    foreach (var contextItem in testItem.Context.TestContextItems)
                    {
                        var contextItemExpander = new Expander();
                        contextItemsStackPanel.Children.Add(contextItemExpander);

                        var expanderHeaderStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                        contextItemExpander.Header = expanderHeaderStackPanel;
                        var contextItemTypeLabel = new Label { Content = "Type: " + contextItem.TypeName };
                        expanderHeaderStackPanel.Children.Add(contextItemTypeLabel);
                        var contextItemUniqueNameLabel = new Label { Content = "UniqueName: " + contextItem.UniqueName };
                        expanderHeaderStackPanel.Children.Add(contextItemUniqueNameLabel);

                        var contextItemXmlType = ReflectionManager.GetXmlTypeByName(contextItem.TypeName);
                        var contextItemTypeDescriptionLabel = new Label { Content = "Description: " + contextItemXmlType.Description };
                        expanderHeaderStackPanel.Children.Add(contextItemTypeDescriptionLabel);
                    }
                }

                if (testItem.Context.CommandManagersItems.Count > 0)
                {
                    var contextManagersExpander = new Expander { Header = "ManagerItems:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
                    contextStackPanel.Children.Add(contextManagersExpander);
                    var contextManagersStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                    contextManagersExpander.Content = contextManagersStackPanel;

                    foreach (var managerItem in testItem.Context.CommandManagersItems)
                    {
                        var managerItemExpander = new Expander();
                        contextManagersStackPanel.Children.Add(managerItemExpander);

                        var expanderHeaderStackPanel = new StackPanel { Margin = new Thickness(5) };
                        managerItemExpander.Header = expanderHeaderStackPanel;
                        var managerItemTypeLabel = new Label { Content = "Type: " + managerItem.ManagerType };
                        expanderHeaderStackPanel.Children.Add(managerItemTypeLabel);
                        var managerItemUniqueNameLabel = new Label { Content = "Name: " + managerItem.Name };
                        expanderHeaderStackPanel.Children.Add(managerItemUniqueNameLabel);

                        var managerItemType = ReflectionManager.GetCommandManagerByTypeName(managerItem.ManagerType);
                        var managerItemTypeDescriptionLabel = new Label { Content = "Description: " + managerItemType.Description };
                        expanderHeaderStackPanel.Children.Add(managerItemTypeDescriptionLabel);
                    }
                }
            }

            //Show TestItem steps

            if (testItem.Steps.Count > 0)
            {
                var stepsExpander = new Expander { Header = "Steps:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
                FInfoPanel.Children.Add(stepsExpander);
                var stepsStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                stepsExpander.Content = stepsStackPanel;

                foreach (var step in testItem.Steps)
                {
                    var stepExpander = new Expander();
                    stepsStackPanel.Children.Add(stepExpander);
                    var stepExpanderHeaderStackPanel = new StackPanel();
                    stepExpander.Header = stepExpanderHeaderStackPanel;

                    var stepNameLabel = new Label { Content = "Name: " + step.Name };
                    stepExpanderHeaderStackPanel.Children.Add(stepNameLabel);
                    var stepDescriptionLabel = new Label { Content = "Description: " + step.Description };
                    stepExpanderHeaderStackPanel.Children.Add(stepDescriptionLabel);
                    var stepPhraseLabel = new Label { Content = "Phrase: " + step.Phrase };
                    stepExpanderHeaderStackPanel.Children.Add(stepPhraseLabel);
                }
            }

            //Show TestItem items 

            var suiteItem = testItem as TestSuite;
            if (suiteItem != null)
            {
                var testItemsExpander = new Expander { Header = "Children:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
                FInfoPanel.Children.Add(testItemsExpander);
                var testItemsStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
                testItemsExpander.Content = testItemsStackPanel;

                foreach (var child in suiteItem.TestItems)
                {
                    var childExpander = new Expander();
                    testItemsStackPanel.Children.Add(childExpander);
                    var childExpanderHeaderStackPanel = new StackPanel();
                    childExpander.Header = childExpanderHeaderStackPanel;

                    var childTypeLabel = new Label { Content = "Type: " + child.ItemType };
                    childExpanderHeaderStackPanel.Children.Add(childTypeLabel);
                    var childNameLabel = new Label { Content = "Name: " + child.Name };
                    childExpanderHeaderStackPanel.Children.Add(childNameLabel);
                    var childDescriptionLabel = new Label { Content = "Description: " + child.Description };
                    childExpanderHeaderStackPanel.Children.Add(childDescriptionLabel);
                }

                testItemsExpander.IsExpanded = true;
            }
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
