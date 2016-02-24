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

    [CommandManager(typeof(TutorialConfig), Description = "Manager for tutorial")]
    public class TutorialManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public string tempDir { get; set; }
        }

        ThreadLocal<LocalContainer> _container;

        public TutorialManager(TutorialConfig config)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.tempDir = config.Folder;
                return localContainer;
            });
        }

        public void CreatePage(Tutorial tutorial)
        {
            foreach (var tutorialItem in tutorial.TutorialItems)
            {
                if (!Directory.Exists(tutorialItem.FolderName))
                    Directory.CreateDirectory(tutorialItem.FolderName);

                foreach (var tutorialFile in tutorialItem.TutorialFiles)
                {
                    string[] lines =
                    {
                        "---",
                        $"title: {tutorialFile.Title}",
                        $"description: {tutorialFile.Description}",
                        $"tags: {tutorialFile.Tags.ToString()}",
                        "---",
                        $"{tutorialFile.Content}"
                    };
                    string file = Path.Combine(_container.Value.tempDir, tutorialItem.FolderName, tutorialFile.Name);
                    File.WriteAllLines(file, lines, Encoding.UTF8);
                }
            }
        }
    }
}
