namespace QA.TestLibs.TestLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TestLogger : ILogger
    {
        private Lazy<NLog.Logger> _log;
        private TestLogger _parentLogger;
        public List<LogMessage> Messages { get; set; } = new List<LogMessage>();

        public string Name { get; private set; }
        public string TestItemType { get; private set; }

        public string FullName { get { return GetFullName(); } }

        public TestLogger(string name, string testItemType)
        {
            TestItemType = testItemType;
            Name = $"({testItemType}) {name}";

            //_log = new Lazy<NLog.Logger>(() =>
            //{
            //    var log = NLog.LogManager.GetLogger(FullName);

            //    return log;
            //});
        }

        public void SetParent(TestLogger log)
        {
            _parentLogger = log;
        }

        public string GetFullName()
        {
            if (_parentLogger == null)
                return $"{Name}";
            return $"{_parentLogger.GetFullName()}${Name}";
        }

        public void TRACE(string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = "TRACE", Message = message, ex = exception });
            _log.Value.Trace(exception, message);
        }

        public void DEBUG(string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = "DEBUG", Message = message, ex = exception });
            _log.Value.Debug(exception, message);
        }

        public void WARN(string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = "WARN", Message = message, ex = exception });
            _log.Value.Warn(exception, message);
        }

        public void INFO(string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = "INFO", Message = message, ex = exception });
            _log.Value.Info(exception, message);
            _parentLogger?.INFO(message, exception);
        }

        public void ERROR(string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = "ERROR", Message = message, ex = exception });
            _log.Value.Error(exception, message);
            _parentLogger?.ERROR(message, exception);
        }

        public void LOG(string level, string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = level, Message = message, ex = exception });
        }
    }
}
