namespace QA.TestLibs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using XmlDesiarilization;
    using WpfControls.CustomFillers;

    public interface IWpfControlFiller
    {
        bool IsMatch(Type type);

        List<BaseValueFiller> GetValueFillers();

        void FillInfoMode(Panel parentPanel, object obj);
        void FillEditMode(Panel parentPanel, object obj);
        void FillCreateMode(Panel parentPanel, Type type);

        Func<object> GetValue { get; }
        Action<object> SetValue { get; }
    }
}
