﻿namespace SAP.Automation.TutorialManager
{
    using QA.TestLibs;
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

        [Command("Create page", Description = "Create tutorial page")]
        public void CreatePage(Tutorial tutorial, ILogger log)
        {
            try
            {
                log?.DEBUG($"Create tutorial page");
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
                log?.DEBUG($"Creating tutorial page completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during creating tutorial page");
                throw new CommandAbortException($"Error occurred during creating tutorial page", ex);
            }
        }
    }
}
