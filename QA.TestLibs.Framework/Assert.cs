namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Commands;
    using XmlDesiarilization;

    [CommandManager(null, "Assert", "AssertManager")]
    public class Assert : CommandManagerBase
    {
        public Assert()
            : base(null)
        {

        }

        [Command("AssertFail", "Fail", Description = "Force fail command")]
        public void Fail(string message, ILogger log)
        {
            log.ERROR(message);
            throw new CommandAbortException(message);
        }
    }
}
