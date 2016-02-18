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
        private Dictionary<TestLogger, TestLogLevel> _parentLoggers = new Dictionary<TestLogger, TestLogLevel>();
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

        public void AddParent(TestLogger log, TestLogLevel level = TestLogLevel.ERROR)
        {
            _parentLoggers.Add(log, level);
            
        }

        public string GetFullName()
        {
            if (_parentLoggers.Count != 0)
                return $"{Name}";
            return $"{_parentLoggers.First().Key.GetFullName()}${Name}";
        }

        public void TRACE(string message, Exception exception = null)
        {
            _log?.Value.Trace(exception, message);
            LOG(TestLogLevel.TRACE, message, exception);
        }

        public void DEBUG(string message, Exception exception = null)
        {
            _log?.Value.Debug(exception, message);
            LOG(TestLogLevel.DEBUG, message, exception);
        }

        public void WARN(string message, Exception exception = null)
        {
            _log?.Value.Warn(exception, message);
            LOG(TestLogLevel.WARN, message, exception);
        }

        public void INFO(string message, Exception exception = null)
        {
            _log?.Value.Info(exception, message);
            LOG(TestLogLevel.INFO, message, exception);
        }

        public void ERROR(string message, Exception exception = null)
        {
            _log?.Value.Error(exception, message);
            LOG(TestLogLevel.ERROR, message, exception);
        }

        protected void LOG(TestLogLevel level, string message, Exception exception = null)
        {
            Messages.Add(new LogMessage { Level = level.ToString(), Message = message, Ex = exception });
            foreach (var log in _parentLoggers)
            {
                if (log.Value >= level)
                {
                    switch (level)
                    {
                        case TestLogLevel.TRACE:
                            log.Key.TRACE(message, exception);
                            break;
                        case TestLogLevel.DEBUG:
                            log.Key.DEBUG(message, exception);
                            break;
                        case TestLogLevel.WARN:
                            log.Key.WARN(message, exception);
                            break;
                        case TestLogLevel.INFO:
                            log.Key.INFO(message, exception);
                            break;
                        case TestLogLevel.ERROR:
                            log.Key.ERROR(message, exception);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SpamToLog(TestLogger log)
        {
            foreach (var message in Messages)
            {
                switch (message.Level)
                {
                    case "TRACE":
                        log.TRACE(message.Message, message.Ex);
                        break;
                    case "DEBUG":
                        log.DEBUG(message.Message, message.Ex);
                        break;
                    case "WARN":
                        log.WARN(message.Message, message.Ex);
                        break;
                    case "INFO":
                        log.INFO(message.Message, message.Ex);
                        break;
                    case "ERROR":
                        log.ERROR(message.Message, message.Ex);
                        break;
                    default:
                        break;
                }
            }
        }

        public enum TestLogLevel
        {
            TRACE, DEBUG, WARN, INFO, ERROR
        }
    }
}
