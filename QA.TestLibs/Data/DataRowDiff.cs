namespace QA.TestLibs.Data
{
    /// <summary>
    /// One diff item of DataRow
    /// </summary>
    public class DataRowDiff
    {
        /// <summary>
        /// Name of column of field witch has different value
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Expected value
        /// </summary>
        public object ExpectedValue { get; set; }
        /// <summary>
        /// Actual value
        /// </summary>
        public object ActualValue { get; set; }
    }
}
