namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class TaskExecutingException : TestLibsException
    {
        public TaskExecutingException(string taskDescription, Exception innerException = null)
            : base($"Error occurs during executing task: {taskDescription}", innerException)
        {

        }
    }
}
