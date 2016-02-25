namespace QA.TestLibs.ApiManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    public class Response : XmlBaseType
    {
        [XmlProperty("Response content")]
        public string Content { get; set; } = null;
    }
}
