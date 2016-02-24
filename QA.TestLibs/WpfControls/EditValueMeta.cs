namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using QA.TestLibs.XmlDesiarilization;

    public class EditValueMeta : BaseEditMeta
    {
        public UIElement Container { get; set; }

        public EditValueMeta(Button editButton)
            : base(editButton)
        {
        }

        protected override void SaveEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            CreatedValue = ControlGetValue();
            if (CreatedValue != SavedValue)
            {
                ControlSetValue(CreatedValue);
                ParentProperty.SetValue(ParentObject, CreatedValue);
            }

            Container.IsEnabled = false;
            SaveButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            e.Handled = true;
        }
        protected override void CancelEditValueButton_Click(object sender, RoutedEventArgs e)
        {
            var newValue = ControlGetValue();
            if (newValue != SavedValue)
                ControlSetValue(SavedValue);

            Container.IsEnabled = false;
            SaveButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            e.Handled = true;
        }
        protected override void EditValueButton_Click(object sender, RoutedEventArgs e)
        {
            Container.IsEnabled = true;
            SaveButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Hidden;
            SavedValue = ControlGetValue();

            CancelButton.Click += CancelEditValueButton_Click;
            SaveButton.Click += SaveEditValueButton_Click;
            e.Handled = true;
        }
    }
}
