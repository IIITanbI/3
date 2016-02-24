namespace QA.TestLibs.GitManager
{
    using System;
    using System.Collections.Generic;
    using XmlDesiarilization;

    [Serializable]
    [XmlType("Git configuration")]
    public class GitConfig : XmlBaseType
    {
        [XmlProperty("Remote repository URI")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string RemoteRepository { get; set; } = null;

        [XmlProperty("Path to local repository")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string LocalRepository { get; set; } = null;

        [XmlProperty("Git user name")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Username { get; set; } = null;

        [XmlProperty("Git password")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Password { get; set; } = null;

        [XmlProperty("User email")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute)]
        public string Email { get; set; } = null;
    }
}
