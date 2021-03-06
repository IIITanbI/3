﻿namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using XmlDesiarilization;
    using WpfControls;
    using System.Windows.Media;
    using System.Windows;

    [WpfControlFiller(typeof(TestItem))]
    public class TestItemControlFiller : BaseXmlTypeControlFiller
    {
        //public void FillInfoMode(Panel parentPanel, XmlBaseType xmlTypeObject)
        //{
        //    var testItem = (TestItem)xmlTypeObject;
        //    //Show TestItem name
        //    var nameLabel = new Label { Content = $"{testItem.ItemType}: {testItem.Name}" };
        //    parentPanel.Children.Add(nameLabel);

        //    //Show test item description
        //    var descriptionGroupBox = new GroupBox { Header = "Description:" };
        //    var descriptionLabel = new Label { Content = testItem.Description };
        //    descriptionGroupBox.Content = descriptionLabel;
        //    parentPanel.Children.Add(descriptionGroupBox);

        //    //Show TestItem context
        //    if (testItem.Context.CommandManagersItems.Count > 0 || testItem.Context.TestContextItems.Count > 0)
        //    {
        //        var contextExpander = new Expander { Header = "Context:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
        //        parentPanel.Children.Add(contextExpander);
        //        var contextStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
        //        contextExpander.Content = contextStackPanel;

        //        if (testItem.Context.TestContextItems.Count > 0)
        //        {
        //            var contextItemsExpander = new Expander { Header = "ContextItems:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
        //            contextStackPanel.Children.Add(contextItemsExpander);
        //            var contextItemsStackPanel = new StackPanel { Margin = new Thickness(5) };
        //            contextItemsExpander.Content = contextItemsStackPanel;

        //            foreach (var contextItem in testItem.Context.TestContextItems)
        //            {
        //                var contextItemExpander = new Expander();
        //                contextItemsStackPanel.Children.Add(contextItemExpander);

        //                var expanderHeaderStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
        //                contextItemExpander.Header = expanderHeaderStackPanel;
        //                var contextItemTypeLabel = new Label { Content = "Type: " + contextItem.TypeName };
        //                expanderHeaderStackPanel.Children.Add(contextItemTypeLabel);
        //                var contextItemUniqueNameLabel = new Label { Content = "UniqueName: " + contextItem.UniqueName };
        //                expanderHeaderStackPanel.Children.Add(contextItemUniqueNameLabel);

        //                var contextItemXmlType = ReflectionManager.GetXmlTypeByName(contextItem.TypeName);
        //                var contextItemTypeDescriptionLabel = new Label { Content = "Description: " + contextItemXmlType.Description };
        //                expanderHeaderStackPanel.Children.Add(contextItemTypeDescriptionLabel);

        //                var contentStackPanel = new StackPanel();
        //                contextItemExpander.Content = contentStackPanel;

        //                var controlFiller = ReflectionManager.GetControlFillerForType(ReflectionManager.GetXmlType(contextItem.GetType()));
        //                controlFiller.FillInfo(contentStackPanel, contextItem);
        //            }
        //        }

        //        if (testItem.Context.CommandManagersItems.Count > 0)
        //        {
        //            var contextManagersExpander = new Expander { Header = "ManagerItems:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
        //            contextStackPanel.Children.Add(contextManagersExpander);
        //            var contextManagersStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
        //            contextManagersExpander.Content = contextManagersStackPanel;

        //            foreach (var managerItem in testItem.Context.CommandManagersItems)
        //            {
        //                var managerItemExpander = new Expander();
        //                contextManagersStackPanel.Children.Add(managerItemExpander);

        //                var expanderHeaderStackPanel = new StackPanel { Margin = new Thickness(5) };
        //                managerItemExpander.Header = expanderHeaderStackPanel;
        //                var managerItemTypeLabel = new Label { Content = "Type: " + managerItem.ManagerType };
        //                expanderHeaderStackPanel.Children.Add(managerItemTypeLabel);
        //                var managerItemUniqueNameLabel = new Label { Content = "Name: " + managerItem.Name };
        //                expanderHeaderStackPanel.Children.Add(managerItemUniqueNameLabel);

        //                var managerItemType = ReflectionManager.GetCommandManagerByTypeName(managerItem.ManagerType);
        //                var managerItemTypeDescriptionLabel = new Label { Content = "Description: " + managerItemType.Description };
        //                expanderHeaderStackPanel.Children.Add(managerItemTypeDescriptionLabel);

        //                var contentStackPanel = new StackPanel();
        //                managerItemExpander.Content = contentStackPanel;

        //                var controlFiller = ReflectionManager.GetControlFillerForType(ReflectionManager.GetXmlType(managerItem.GetType()));
        //                controlFiller.FillInfo(contentStackPanel, managerItem);
        //            }
        //        }
        //    }

        //    //Show TestItem steps

        //    if (testItem.Steps.Count > 0)
        //    {
        //        var stepsExpander = new Expander { Header = "Steps:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
        //        parentPanel.Children.Add(stepsExpander);
        //        var stepsStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
        //        stepsExpander.Content = stepsStackPanel;

        //        foreach (var step in testItem.Steps)
        //        {
        //            var stepExpander = new Expander();
        //            stepsStackPanel.Children.Add(stepExpander);
        //            var stepExpanderHeaderStackPanel = new StackPanel();
        //            stepExpander.Header = stepExpanderHeaderStackPanel;

        //            var stepNameLabel = new Label { Content = "Name: " + step.Name };
        //            stepExpanderHeaderStackPanel.Children.Add(stepNameLabel);
        //            var stepDescriptionLabel = new Label { Content = "Description: " + step.Description };
        //            stepExpanderHeaderStackPanel.Children.Add(stepDescriptionLabel);
        //            var stepPhraseLabel = new Label { Content = "Phrase: " + step.Phrase };
        //            stepExpanderHeaderStackPanel.Children.Add(stepPhraseLabel);
        //        }
        //    }

        //    //Show TestItem items 

        //    var suiteItem = testItem as TestSuite;
        //    if (suiteItem != null)
        //    {
        //        var testItemsExpander = new Expander { Header = "Children:", BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1), Margin = new Thickness(5) };
        //        parentPanel.Children.Add(testItemsExpander);
        //        var testItemsStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
        //        testItemsExpander.Content = testItemsStackPanel;

        //        foreach (var child in suiteItem.TestItems)
        //        {
        //            var childExpander = new Expander();
        //            testItemsStackPanel.Children.Add(childExpander);
        //            var childExpanderHeaderStackPanel = new StackPanel();
        //            childExpander.Header = childExpanderHeaderStackPanel;

        //            var childTypeLabel = new Label { Content = "Type: " + child.ItemType };
        //            childExpanderHeaderStackPanel.Children.Add(childTypeLabel);
        //            var childNameLabel = new Label { Content = "Name: " + child.Name };
        //            childExpanderHeaderStackPanel.Children.Add(childNameLabel);
        //            var childDescriptionLabel = new Label { Content = "Description: " + child.Description };
        //            childExpanderHeaderStackPanel.Children.Add(childDescriptionLabel);


        //            var contentStackPanel = new StackPanel();
        //            childExpander.Content = contentStackPanel;

        //            var controlFiller = ReflectionManager.GetControlFillerForType(ReflectionManager.GetXmlType(child.GetType()));
        //            controlFiller.FillInfo(contentStackPanel, child);
        //        }

        //        testItemsExpander.IsExpanded = true;
        //    }
        //}
    }
}
