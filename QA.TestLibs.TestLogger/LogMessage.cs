using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.TestLibs.TestLogger
{
    public class LogMessage
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public Exception ex { get; set; }
    }
}
