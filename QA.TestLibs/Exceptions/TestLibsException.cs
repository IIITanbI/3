namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class TestLibsException : Exception
    {
        public TestLibsException(string description, Exception innerException = null)
            : base(description, innerException)
        {

        }
    }
}
