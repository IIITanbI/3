namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public class StringFiller : BaseValueFiller
    {
        public override bool IsMatch(Type type)
        {
            return type == typeof(string);
        }

        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var textBlock = new TextBox();
            parentPanel.Children.Add(textBlock);

            GetValue = () => textBlock.Text;
            SetValue = val => textBlock.Text = val?.ToString();
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var textBlock = new TextBox { Text = obj?.ToString() };
            parentPanel.Children.Add(textBlock);

            GetValue = () => textBlock.Text;
            SetValue = val => textBlock.Text = val?.ToString();
        }
    }
}
