namespace QA.TestLibs
{
    public enum TestItemStatus
    {
        NotExecuted, Unknown, Passed, Failed, Skipped
    }

    public enum TestItemType
    {
        Project, Suite, Test
    }

    public enum LogLevel
    {
        TRACE, DEBUG, WARN, INFO, ERROR
    }
}
