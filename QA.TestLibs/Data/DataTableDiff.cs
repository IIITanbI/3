namespace QA.TestLibs.Data
{
    /// <summary>
    /// One diff item of DataTable
    /// </summary>
    public class DataTableDiff
    {
        /// <summary>
        /// Expected row
        /// </summary>
        public DataRow ExpectRow { get; set; }
        /// <summary>
        /// Actual row
        /// </summary>
        public DataRow ActualRow { get; set; }
    }
}
