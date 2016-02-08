namespace QA.TestLibs.Data
{
    using System.Collections.Generic;

    public class DataTable
    {
        /// <summary>
        /// List of DataRows
        /// </summary>
        public List<DataRow> Rows;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataTable()
        {
            Rows = new List<DataRow>();
        }

        /// <summary>
        /// Create DataTables with added specified list of DataRows
        /// </summary>
        /// <param name="dataRows">List of DataRows</param>
        public DataTable(List<DataRow> dataRows)
        {
            Rows = new List<DataRow>();
        }

        /// <summary>
        /// Add DataRow
        /// </summary>
        /// <param name="row">DataRow to add</param>
        public void Add(DataRow row)
        {
            Rows.Add(row);
        }

        /// <summary>
        /// Add DataRow initialized by specified dictionary
        /// </summary>
        /// <param name="values">Dictionary to initilize DataRow</param>
        public void Add(Dictionary<string, object> values)
        {
            Rows.Add(new DataRow(values));
        }

        /// <summary>
        /// Get clone object of current DataTable
        /// </summary>
        /// <returns>Clone object of current DataTable</returns>
        public DataTable Clone()
        {
            var dataTable = new DataTable();

            foreach (var dr in Rows)
                dataTable.Add(dr.Clone());

            return dataTable;
        }
    }
}
