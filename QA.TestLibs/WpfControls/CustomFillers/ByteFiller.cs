namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    public class ByteFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var byteDownUp = new ByteUpDown();
            parentPanel.Children.Add(byteDownUp);

            GetValue = () => byteDownUp.Value.GetValueOrDefault();
            SetValue = val => byteDownUp.Value = (byte)val;
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var byteDownUp = new ByteUpDown { Value = (byte)obj };
            parentPanel.Children.Add(byteDownUp);

            GetValue = () => byteDownUp.Value.GetValueOrDefault();
            SetValue = val => byteDownUp.Value = (byte)val;
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(byte);
        }
    }
}
