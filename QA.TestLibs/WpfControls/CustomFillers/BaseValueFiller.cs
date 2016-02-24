namespace QA.TestLibs.WpfControls.CustomFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    public abstract class BaseValueFiller : IWpfControlFiller
    {
        public Func<object> GetValue { get; protected set; }

        public Action<object> SetValue { get; protected set; }

        public abstract void FillCreateMode(Panel parentPanel, Type type);

        public abstract void FillEditMode(Panel parentPanel, object obj);

        public virtual void FillInfoMode(Panel parentPanel, object obj)
        {
            var label = new Label { Content = (obj ?? "Value is not specified or null").ToString() };
            parentPanel.Children.Add(label);
        }

        public List<BaseValueFiller> GetValueFillers() => null;

        public abstract bool IsMatch(Type type);
    }
}
