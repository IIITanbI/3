namespace QA.TestLibs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    public interface IWpfTypeControl
    {
        void GenerateInfoMode(XmlBaseType obj);
        void GenerateEditMode(XmlBaseType obj);
    }
}
