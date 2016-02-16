﻿namespace QA.TestLibs.TestMetadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.TestLibs.XmlDesiarilization;

    public class TestTry
    {
        [XmlProperty("Test item log")]
        public List<LogMessage> TryLogMessages { get; set; }
    }
}
