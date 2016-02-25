namespace QA.TestLibs.GitManager
{
    using System;
    using System.Collections.Generic;
    using XmlDesiarilization;

    [XmlType("Git configuration")]
    public class GitConfig : XmlBaseType
    {
        [XmlProperty("Remote repository URI")]
        public string RemoteRepository { get; set; } = null;

        [XmlProperty("Path to local repository")]
        public string LocalRepository { get; set; } = null;

        [XmlProperty("Git user name")]
        public string Username { get; set; } = null;

        [XmlProperty("Git password")]
        public string Password { get; set; } = null;

        [XmlProperty("User email")]
        public string Email { get; set; } = null;
    }
}
