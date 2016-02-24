namespace QA.TestLibs.GitManager
{
    using Commands;
    using LibGit2Sharp;
    using System;
    using System.Collections.Generic;
    [CommandManager(typeof(GitConfig), "Git", Description = "Manager for Git")]
    public class GitManager : CommandManagerBase
    {
        public GitConfig Config { get; protected set; }

        public GitManager(GitConfig config)
            : base(config)
        {
            Config = config;
        }

        public void GitClone()
        {
            var cloneOptions = new CloneOptions();
            cloneOptions.CredentialsProvider = (_url, _user, _cred) =>
            new UsernamePasswordCredentials
            {
                Username = Config.Username,
                Password = Config.Password
            };
            Repository.Clone(Config.RemoteRepository, Config.LocalRepository, cloneOptions);
        }

        public void GitPull()
        {
            using (var repository = new Repository(Config.LocalRepository))
            {
                PullOptions pullOptions = new PullOptions();
                pullOptions.FetchOptions = new FetchOptions();
                pullOptions.FetchOptions.CredentialsProvider = (_url, _user, _cred) =>
                new UsernamePasswordCredentials
                {
                    Username = Config.Username,
                    Password = Config.Password
                };
                repository.Network.Pull(new Signature(Config.Username, Config.Email, new DateTimeOffset(DateTime.Now)), pullOptions);
            }
        }

        public void GitAdd(List<string> files, Repository repository)
        {
            foreach (var file in files)
            {
                repository.Index.Add(file);
            }
        }

        public void GitStage(List<string> files, Repository repository)
        {
            foreach (var file in files)
            {
                repository.Stage(file);
            }
        }

        public void GitCommit()
        {
            using (var repository = new Repository(Config.LocalRepository))
            {
                GitAdd(Config.CommitFiles, repository);
                GitStage(Config.CommitFiles, repository);

                Signature author = new Signature(Config.Username, Config.Email, DateTime.Now);
                Commit commit = repository.Commit(Config.CommitMessage, author, author);
            }
        }

        public void GitPush()
        {
            using (var repository = new Repository(Config.LocalRepository))
            {
                Remote remote = repository.Network.Remotes["origin"];
                PushOptions pushOptions = new PushOptions();
                pushOptions.CredentialsProvider = (_url, _user, _cred) =>
                new UsernamePasswordCredentials
                {
                    Username = Config.Username,
                    Password = Config.Password
                };
                repository.Network.Push(remote, @"refs/heads/master", pushOptions);
            }
        }
    }
}
