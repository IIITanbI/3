namespace QA.TestLibs.ApiManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Api configuration")]
    public class ApiManagerConfig : XmlBaseType
    {
        [XmlProperty("Api endpoint")]
        public string EndPoint { get; set; } = null;
    }
}
