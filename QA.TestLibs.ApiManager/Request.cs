namespace QA.TestLibs.ApiManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("Api request")]
    public class Request : XmlBaseType
    {
        [XmlProperty("Request method")]
        public Methods Method { get; set; }

        [XmlProperty("Request content type")]
        public string ContentType { get; set; }

        [XmlProperty("Request post data")]
        public string PostData { get; set; }

        public enum Methods
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
