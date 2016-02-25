namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class IntFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var intUpDown = new IntegerUpDown();
            parentPanel.Children.Add(intUpDown);

            GetValue = () => intUpDown.Value.GetValueOrDefault();
            SetValue = val => intUpDown.Value = (int)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var intUpDown = new IntegerUpDown { Value = (int)obj };
            parentPanel.Children.Add(intUpDown);

            GetValue = () => intUpDown.Value.GetValueOrDefault();
            SetValue = val => intUpDown.Value = (int)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(int);
        }
    }
}
