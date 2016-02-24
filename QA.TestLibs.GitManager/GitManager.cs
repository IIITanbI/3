namespace QA.TestLibs.GitManager
{
    using Commands;
    using LibGit2Sharp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    [CommandManager(typeof(GitConfig), "Git", Description = "Manager for Git")]
    public class GitManager : CommandManagerBase
    {
        private class LocalContainer
        {
            public GitConfig Config;
        }

        ThreadLocal<LocalContainer> _container;

        public GitManager(GitConfig config)
            : base(config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.Config = config;
                return localContainer;
            });
        }

        [Command("Clone", Description = "Clone remote repository")]
        public void Clone(ILogger log)
        {
            try
            {
                log?.DEBUG($"Clone remote repository");
                var cloneOptions = new CloneOptions();
                cloneOptions.CredentialsProvider = (_url, _user, _cred) =>
                new UsernamePasswordCredentials
                {
                    Username = _container.Value.Config.Username,
                    Password = _container.Value.Config.Password
                };
                Repository.Clone(_container.Value.Config.RemoteRepository, _container.Value.Config.LocalRepository, cloneOptions);
                log?.DEBUG($"Cloning remote repository completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during cloning remote repository");
                throw new CommandAbortException($"Error occurred during cloning remote repository", ex);
            }
        }

        [Command("Pull", Description = "Pull changes from remote repository")]
        public void Pull(ILogger log)
        {
            try
            {
                log?.DEBUG($"Pull changes from remote repository");
                using (var repository = new Repository(_container.Value.Config.LocalRepository))
                {
                    PullOptions pullOptions = new PullOptions();
                    pullOptions.FetchOptions = new FetchOptions();
                    pullOptions.FetchOptions.CredentialsProvider = (_url, _user, _cred) =>
                    new UsernamePasswordCredentials
                    {
                        Username = _container.Value.Config.Username,
                        Password = _container.Value.Config.Password
                    };
                    repository.Network.Pull(new Signature(_container.Value.Config.Username, _container.Value.Config.Email, new DateTimeOffset(DateTime.Now)), pullOptions);
                }
                log?.DEBUG($"Pulling changes from remote repository completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during pulling changes from remote repository");
                throw new CommandAbortException($"Error occurred during pulling changes from remote repository", ex);
            }
        }

        [Command("Add", Description = "Add files to local repository index")]
        public void Add(ILogger log)
        {
            try
            {
                log?.DEBUG($"Add files to local repository index");
                using (var repository = new Repository(_container.Value.Config.LocalRepository))
                {
                    var files = Directory.GetFiles(_container.Value.Config.LocalRepository);
                    foreach (var file in files)
                    {
                        repository.Index.Add(file);
                    }
                }
                log?.DEBUG($"Adding files to local repository index completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during adding files to local repository index");
                throw new CommandAbortException($"Error occurred during adding files to local repository index", ex);
            }
        }

        [Command("Commit", Description = "Commit changes to local repository")]
        public void Commit(string commitMessage, ILogger log)
        {
            try
            {
                log?.DEBUG($"Commit changes to local repository");
                using (var repository = new Repository(_container.Value.Config.LocalRepository))
                {
                    Signature author = new Signature(_container.Value.Config.Username, _container.Value.Config.Email, DateTime.Now);
                    Commit commit = repository.Commit(commitMessage, author, author);
                }
                log?.DEBUG($"Committing changes to local repository completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during committing changes to local repository");
                throw new CommandAbortException($"Error occurred during committing changes to local repository", ex);
            }
        }

        [Command("Push", Description = "Push commits to remote repository")]
        public void Push(ILogger log)
        {
            try
            {
                log?.DEBUG($"Push commits to remote repository");
                using (var repository = new Repository(_container.Value.Config.LocalRepository))
                {
                    Remote remote = repository.Network.Remotes["origin"];
                    PushOptions pushOptions = new PushOptions();
                    pushOptions.CredentialsProvider = (_url, _user, _cred) =>
                    new UsernamePasswordCredentials
                    {
                        Username = _container.Value.Config.Username,
                        Password = _container.Value.Config.Password
                    };
                    repository.Network.Push(remote, @"refs/heads/master", pushOptions);
                }
                log?.DEBUG($"Pushing commits to remote repository completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during pushing commits to remote repository");
                throw new CommandAbortException($"Error occurred during pushing commits to remote repository", ex);
            }
        }
    }
}
