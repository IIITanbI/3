namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class BoolFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var checkBox = new CheckBox();
            parentPanel.Children.Add(checkBox);

            GetValue = () => checkBox.IsChecked.GetValueOrDefault();
            SetValue = val => checkBox.IsChecked = (bool)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var checkBox = new CheckBox { IsChecked = (bool)obj };
            parentPanel.Children.Add(checkBox);

            GetValue = () => checkBox.IsChecked.GetValueOrDefault();
            SetValue = val => checkBox.IsChecked = (bool)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(bool);
        }
    }
}
