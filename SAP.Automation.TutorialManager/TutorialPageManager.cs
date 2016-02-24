namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs.Commands;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(typeof(TutorialConfig), "WebDriver", Description = "Manager for WebDriver")]
    public class TutorialPageManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public TutorialConfig Config;
        }

        ThreadLocal<LocalContainer> _container;

        public TutorialPageManager(TutorialConfig config)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.Config = config;
                return localContainer;
            });
        }

        public void CreatePage(TutorialFile tutorialFile)
        {
            string tempDir = _container.Value.Config.Folder;
            string tempFile = Path.Combine(_container.Value.Config.Folder, tutorialFile.Name);

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string[] lines =
            {
                "---",
                $"title: {tutorialFile.Title}",
                $"description: {tutorialFile.Description}",
                $"tags: {tutorialFile.Tags.ToString()}",
                "---",
                $"{tutorialFile.Content}"
            };

            File.WriteAllLines(tempFile, lines, Encoding.UTF8);
        }
    }
}
