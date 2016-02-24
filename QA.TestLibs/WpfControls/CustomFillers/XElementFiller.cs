namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Xml.Linq;

    public class XElementFiller : BaseValueFiller
    {
        public override void FillCreateMode(Panel parentPanel, Type type)
        {
            var textBox = new TextBox();
            parentPanel.Children.Add(textBox);

            GetValue = () => XElement.Parse(textBox.Text);
            SetValue = val => textBox.Text = val.ToString();
        }

        public override void FillEditMode(Panel parentPanel, object obj)
        {
            var textBox = new TextBox { Text = obj?.ToString() };
            parentPanel.Children.Add(textBox);

            GetValue = () => XElement.Parse(textBox.Text);
            SetValue = val => textBox.Text = val.ToString();
        }

        public override bool IsMatch(Type type)
        {
            return type == typeof(XElement);
        }
    }
}
