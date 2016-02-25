namespace QA.TestLibs.ApiManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Api manager configuration")]
    public class ApiManagerConfig : XmlBaseType
    {
        [XmlProperty("Api endpoint")]
        public string EndPoint { get; set; } = null;

        [XmlProperty("Api user name")]
        public string Username { get; set; } = null;

        [XmlProperty("Api password")]
        public string Password { get; set; } = null;
    }
}
