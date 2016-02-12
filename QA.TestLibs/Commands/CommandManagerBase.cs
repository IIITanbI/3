namespace QA.TestLibs.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [CommandManager(typeof(XmlBaseType), "BaseCommandManager", Description = "Base class for all command managers")]
    public abstract class CommandManagerBase
    {
        public CommandManagerBase(XmlBaseType configurationObject)
        {

        }
    }
}
