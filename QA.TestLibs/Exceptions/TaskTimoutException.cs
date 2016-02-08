namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class TaskTimoutException : TestLibsException
    {
        public TaskTimoutException(string taskDescription, int timeout, Exception innerException = null)
            : base($"Timeout: {timeout} was riched during executing task: {taskDescription}", innerException)
        {

        }
    }
}
