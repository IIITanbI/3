namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;


    public class DoubleFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var doubleUpDown = new DoubleUpDown();
            parentPanel.Children.Add(doubleUpDown);

            GetValue = () => doubleUpDown.Value.GetValueOrDefault();
            SetValue = val => doubleUpDown.Value = (double)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var doubleUpDown = new DoubleUpDown { Value = (double)obj };
            parentPanel.Children.Add(doubleUpDown);

            GetValue = () => doubleUpDown.Value.GetValueOrDefault();
            SetValue = val => doubleUpDown.Value = (double)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(double);
        }
    }
}
