namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class DateTimeFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var dateTimeUpDown = new DateTimeUpDown();
            parentPanel.Children.Add(dateTimeUpDown);

            GetValue = () => dateTimeUpDown.Value.GetValueOrDefault();
            SetValue = val => dateTimeUpDown.Value = (DateTime)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var dateTimeUpDown = new DateTimeUpDown { Value = (DateTime)obj };
            parentPanel.Children.Add(dateTimeUpDown);

            GetValue = () => dateTimeUpDown.Value.GetValueOrDefault();
            SetValue = val => dateTimeUpDown.Value = (DateTime)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(DateTime);
        }
    }
}
