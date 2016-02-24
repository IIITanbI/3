namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class EnumFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var values = Enum.GetNames(type);
            var comboBox = new ComboBox();
            parentPanel.Children.Add(comboBox);
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
            comboBox.SelectedIndex = 0;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            GetValue = () => Enum.Parse(type, comboBox.SelectedItem.ToString());
            SetValue = val => comboBox.SelectedIndex = values.ToList().IndexOf(val.ToString());
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var type = obj.GetType();
            var values = Enum.GetNames(type);
            var comboBox = new ComboBox();
            parentPanel.Children.Add(comboBox);
            foreach (var value in values)
            {
                comboBox.Items.Add(value);
            }
            comboBox.SelectedIndex = values.ToList().IndexOf(obj.ToString());
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            
            GetValue = () => Enum.Parse(type, comboBox.SelectedItem.ToString());
            SetValue = val => comboBox.SelectedIndex = values.ToList().IndexOf(val.ToString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        public override bool IsMatch(Type type)
        {
            return type.IsEnum;
        }
    }
}
