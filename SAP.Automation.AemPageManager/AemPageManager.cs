namespace SAP.Automation.AemPageManager
{
    using QA.TestLibs;
    using QA.TestLibs.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(null, Description = "Manager for aem pages")]
    public class AemPageManager : CommandManagerBase
    {
        public AemPageManager()
            : base(null)
        { }

        [Command("CreatePageCmd", Description = "Generate command for aem page creation")]
        public string GenerateCommandForCreatePage(AemPage aemPage, ILogger log)
        {
            try
            {
                log?.DEBUG($"Generate command for aem page '{aemPage.Title}' creation");

                var tmp = $"?cmd=createPage&parentPath={aemPage.ParentPath}&title={aemPage.Title}&template={aemPage.Template}";

                log?.DEBUG($"Generating command for aem page '{aemPage.Title}' creation completed");

                return tmp;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' creation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' creation", ex);
            }
        }

        [Command("ActivatePageCmd", Description = "Generate command for aem page activation")]
        public string GenerateCommandForActivatePage(AemPage aemPage, ILogger log)
        {
            try
            {
                log?.DEBUG($"Generate command for aem page '{aemPage.Title}' activation");

                var tmp = $"?cmd=Activate&path={aemPage.ParentPath}/{aemPage.Title}";

                log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");

                return tmp;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' activation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' activation", ex);
            }
        }
    }
}
