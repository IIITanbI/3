namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.TestLibs.ExtensionMethods;
    using System.Xml.Linq;

    /// <summary>
    /// Exception for any missed configuration element. Include description of missed element responsibility
    /// </summary>
    public class MissedConfigElementException : TestLibsException
    {
        public MissedConfigElementException(string elementName, string description, XElement parentElement, Exception innerException = null)
            : base($"There is no required config element/attribute with name: {elementName} in parent element:\n{parentElement.ToString()}\nthat is responsible for:\n\t{description}\n", innerException)
        {
        }

        public MissedConfigElementException(List<string> elementNames, string description, XElement parentElement, Exception innerException = null)
           : base($"There is no required config element/attribute in parent element:\n{parentElement.ToString()}\nthat is responsible for:\n\t{description}\npossible element/attribute names:\n{elementNames.ToStringWithList()}", innerException)
        {
        }
    }
}
