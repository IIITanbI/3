namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(typeof(TutorialConfig), "WebDriver", Description = "Manager for WebDriver")]
    public class TutorialPageManager : CommandManagerBase
    {
        public TutorialPageManager(TutorialConfig config)
            : base(config)
        {
        }
    }
}
