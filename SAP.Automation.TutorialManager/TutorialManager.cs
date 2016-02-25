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

    [CommandManager(typeof(TutorialManagerConfig), Description = "Manager for tutorial")]
    public class TutorialManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public string tempDir { get; set; }
        }

        ThreadLocal<LocalContainer> _container;

        public TutorialManager(TutorialManagerConfig config)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.tempDir = config.Folder;

                if (!Directory.Exists(localContainer.tempDir))
                    Directory.CreateDirectory(localContainer.tempDir);

                return localContainer;
            });
        }

        public void CreatePage(Tutorial tutorial)
        {
            var tutorialPath = Path.Combine(_container.Value.tempDir, tutorial.Folder);

            if (!Directory.Exists(tutorialPath))
                Directory.CreateDirectory(tutorialPath);

            foreach (var tutorialItem in tutorial.TutorialItems)
            {
                var tutorialItemPath = Path.Combine(tutorialPath, tutorialItem.FolderName);

                if (!Directory.Exists(tutorialItemPath))
                    Directory.CreateDirectory(tutorialItemPath);

                foreach (var tutorialFile in tutorialItem.TutorialFiles)
                {
                    var listTags = new StringBuilder();
                    //tutorialFile.Tags.ForEach(x => listTags.Append(x + " "));

                    for (int i = 0; i < tutorialFile.Tags.Count; i++)
                    {
                        if (i != tutorialFile.Tags.Count - 1)
                            listTags.Append(tutorialFile.Tags[i] + " ");
                        else
                            listTags.Append(tutorialFile.Tags[i] + ";");
                    }

                    string[] lines =
                    {
                        "---",
                        $"title: {tutorialFile.Title}",
                        $"description: {tutorialFile.Description}",
                        $"tags: {listTags}",
                        "---",
                        $"{tutorialFile.Content}"
                    };


                    string file = Path.Combine(tutorialItemPath, tutorialFile.Name + ".md");
                    File.WriteAllLines(file, lines, Encoding.UTF8);
                }
            }
        }
    }
}
