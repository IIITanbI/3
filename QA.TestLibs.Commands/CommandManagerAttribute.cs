namespace QA.TestLibs.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandManagerAttribute : Attribute
    {
        public List<string> Names { get; private set; }
        public string Description { get; set; }

        public CommandManagerAttribute(params string[] names)
        {
            Names = names.ToList();
        }
    }
}
