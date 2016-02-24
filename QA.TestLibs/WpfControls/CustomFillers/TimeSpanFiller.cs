namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;


    public class TimeSpanFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var timSpanUpDown = new TimeSpanUpDown();
            parentPanel.Children.Add(timSpanUpDown);

            GetValue = () => timSpanUpDown.Value.GetValueOrDefault();
            SetValue = val => timSpanUpDown.Value = (TimeSpan)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var timSpanUpDown = new TimeSpanUpDown { Value = (TimeSpan)obj };
            parentPanel.Children.Add(timSpanUpDown);

            GetValue = () => timSpanUpDown.Value.GetValueOrDefault();
            SetValue = val => timSpanUpDown.Value = (TimeSpan)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(TimeSpan);
        }
    }
}
