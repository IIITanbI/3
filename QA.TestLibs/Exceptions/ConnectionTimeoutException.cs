namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Exception for connection timeout
    /// </summary>
    public class ConnectionTimeoutException : TestLibsException
    {
        /// <param name="instanceTypeToConnect">Type of instance to which connection has been tried to made</param>
        /// <param name="timeout">Timeout in ms</param>
        /// <param name="connectionInfo">Connection string or some connection info</param>
        public ConnectionTimeoutException(string instanceTypeToConnect, int timeout, string connectionInfo, Exception innerException = null)
            : base($"Timeout: {timeout} was riched during connection to: {instanceTypeToConnect} using: {connectionInfo}", innerException)
        {

        }
    }
}
