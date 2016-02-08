namespace QA.TestLibs.Data
{
    using QA.TestLibs.Xml;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using ExtensionMethods;

    /// <summary>
    /// Allow to compare DataRows and DataTables bu specified columns
    /// </summary>
    public class DataComparator
    {
        /// <summary>
        /// List of unique columns. Used to find similar DataRows in DataTables
        /// </summary>
        [ConfigElementLocation("unique", "uniqueColumns", "uColumns", "uniqColumns")]
        [ConfigElementLocation("uniqueFilds", "uFilds", "uniqFields")]
        [ConfigElementLocation("uniqueElements", "uniqElements", "uElements", "uEls")]
        [ConfigChildElementLocation("element", "column", "field", "el")]
        [ConfigElement("List of unique columns. Used to find similar DataRows in DataTables")]
        public List<string> UniqueColumns { get; set; }

        /// <summary>
        /// List of columns that should be compared. If empty all columns will be compared
        /// </summary>
        [ConfigElementLocation("compare", "comparable", "comparableColumns", "cColumns", "compColumns", "comparColumns")]
        [ConfigElementLocation("comparableFilds", "cFilds", "comparFields", "fieldsToCompare")]
        [ConfigElementLocation("comparableElements", "cElements", "compElements", "cEls")]
        [ConfigChildElementLocation("element", "column", "field", "el")]
        [ConfigElement("List of columns that should be compared. If empty all columns will be compared")]
        public List<string> ComparableColumns { get; set; }

        /// <summary>
        /// List of colums that shouldn't be compared.
        /// </summary>
        [ConfigElementLocation("ignore", "ignorable", "ignorableColumns", "iColumns", "ignorColumns")]
        [ConfigElementLocation("ignorableFilds", "iFilds", "ignorFilds", "fieldsToIgnore")]
        [ConfigElementLocation("ignorableElements", "iElements", "ignorElements", "iEls")]
        [ConfigChildElementLocation("element", "column", "field", "el")]
        [ConfigElement("List of colums that shouldn't be compared")]
        public List<string> IgnorableColumns { get; set; }

        /// <summary>
        /// Default constructor. Initialize UniqueColumns, ComparableColumns and IgnorableColumns with empty lists.
        /// </summary>
        public DataComparator()
        {
            UniqueColumns = new List<string>();
            ComparableColumns = new List<string>();
            IgnorableColumns = new List<string>();
        }
        /// <summary>
        /// Constructor with specified UniqueColumns, ComparableColumns and IgnorableColumns
        /// </summary>
        /// <param name="uniqueColumns">List of unique columns</param>
        /// <param name="comparableColumns">List of comparable columns</param>
        /// <param name="ignorableColumns">List of ignorable columns</param>
        public DataComparator(List<string> uniqueColumns, List<string> comparableColumns, List<string> ignorableColumns)
        {
            UniqueColumns = uniqueColumns ?? new List<string>();
            ComparableColumns = comparableColumns ?? new List<string>();
            IgnorableColumns = ignorableColumns ?? new List<string>();
        }

        /// <summary>
        /// Compare two rows using Comparable and Ignorable columns by string representation of values. If Comparable columns are empty, compare all columns.
        /// </summary>
        /// <param name="whatRow">What row compare</param>
        /// <param name="withRow">With what row compare</param>
        /// <returns>True if all values are the same</returns>
        public bool RowCompareDataRows(DataRow whatRow, DataRow withRow)
        {
            if (ComparableColumns.Count > 0)
            {
                foreach (var cc in ComparableColumns)
                {
                    if (IgnorableColumns.Contains(cc)) continue;

                    var wav = whatRow[cc, true];
                    var wiv = withRow[cc, true];
                    if (wav != wiv) return false;
                }
                return true;
            }

            var columns = whatRow.Columns;
            foreach (var column in columns)
            {
                if (IgnorableColumns.Contains(column)) continue;

                var wav = whatRow[column, true];
                var wiv = withRow[column, true];
                if (wav != wiv) return false;
            }

            var diffs = RowGetDiffValues(whatRow, withRow);
            return diffs.Count == 0;
        }

        /// <summary>
        /// Compare values of Unique columns
        /// </summary>
        /// <param name="whatRow">What row compare</param>
        /// <param name="withRow">With what row compare</param>
        /// <returns>True if values of Unique columns are the same</returns>
        public bool RowIsUniqsSame(DataRow whatRow, DataRow withRow)
        {
            foreach (var uc in UniqueColumns)
            {
                var wav = whatRow[uc, true];
                var wiv = withRow[uc, true];
                if (wav != wiv) return false;
            }
            return true;
        }

        /// <summary>
        /// Get list of DataRowDiff objects.
        /// </summary>
        /// <param name="whatRow">What row compare</param>
        /// <param name="withRow">With what row compare</param>
        /// <param name="withoutExtraAndMissed">True if need to compare just columns from what rows. Default: false</param>
        /// <returns>List of DataRowDiff objects</returns>
        public List<DataRowDiff> RowGetDiffValues(DataRow whatRow, DataRow withRow, bool withoutExtraAndMissed = false)
        {
            var difs = new List<DataRowDiff>();

            if (ComparableColumns.Count > 0)
            {
                foreach (var cc in ComparableColumns)
                {
                    if (IgnorableColumns.Contains(cc)) continue;

                    var wav = whatRow[cc, true];
                    var wiv = withRow[cc, true];
                    if (withoutExtraAndMissed && ((wav == null && wiv != null) || (wav != null && wiv == null))) continue;
                    if (wav != wiv) difs.Add(new DataRowDiff { Column = cc, ExpectedValue = whatRow[cc], ActualValue = withRow[cc] });
                }
                return difs;
            }

            var columns = whatRow.Columns;
            foreach (var column in columns)
            {
                if (IgnorableColumns.Contains(column)) continue;

                var wav = whatRow[column, true];
                var wiv = withRow[column, true];
                if (withoutExtraAndMissed && ((wav == null && wiv != null) || (wav != null && wiv == null))) continue;
                if (wav != wiv) difs.Add(new DataRowDiff { Column = column, ExpectedValue = whatRow[column], ActualValue = withRow[column] });
            }
            return difs;
        }

        /// <summary>
        /// Get Dictionary of Extra values. (Additional values from withRow that are not contained in whatRow)
        /// </summary>
        /// <param name="whatRow">What row compare</param>
        /// <param name="withRow">With what row compare</param>
        /// <returns>Dictionary of Extra values</returns>
        public Dictionary<string, object> RowGetExtraValues(DataRow whatRow, DataRow withRow)
        {
            var extra = new Dictionary<string, object>();

            var columns = withRow.Columns;
            foreach (var column in columns)
            {
                if (IgnorableColumns.Contains(column)) continue;

                var wav = whatRow[column, true];
                if (wav == null) extra.Add(column, withRow[column]);
            }
            return extra;
        }

        /// <summary>
        /// Get Dictionary of Missed values. (Missed values from whatRow that are not contained in withRow)
        /// </summary>
        /// <param name="whatRow">What row compare</param>
        /// <param name="withRow">With what row compare</param>
        /// <returns>Dictionary of Extra values</returns>
        public Dictionary<string, object> RowGetMissedValues(DataRow whatRow, DataRow withRow)
        {
            var missed = new Dictionary<string, object>();

            var columns = whatRow.Columns;
            foreach (var column in columns)
            {
                if (IgnorableColumns.Contains(column)) continue;

                var wiv = withRow[column, true];
                if (wiv == null) missed.Add(column, whatRow[column]);
            }
            return missed;
        }

        /// <summary>
        /// Search simmilar (bu unique column values) DataRow in specified table
        /// </summary>
        /// <param name="whatRow">What DataRow</param>
        /// <param name="whereTable">In what DataTable</param>
        /// <returns>DataRow with the same unique field values</returns>
        public DataRow TableFindSimmilarRow(DataRow whatRow, DataTable whereTable)
        {
            foreach (var row in whereTable.Rows)
            {
                if (RowIsUniqsSame(whatRow, row))
                    return row;
            }
            return null;
        }

        /// <summary>
        /// Compare two tables by searching similar rows.
        /// </summary>
        /// <param name="whatTable">What DataTable</param>
        /// <param name="withTable">With what DataTable</param>
        /// <returns>True if all DataRows from whatTable present in withTable (JUST has the same unique column values)</returns>
        public bool TableCompareWithTable(DataTable whatTable, DataTable withTable)
        {
            foreach (var war in whatTable.Rows)
            {
                if (TableFindSimmilarRow(war, withTable) == null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get list of DataTableDiff objects.
        /// </summary>
        /// <param name="whatTable">What DataTable</param>
        /// <param name="withTable">With what DataTable</param>
        /// <returns>List of DataTableDiff objects. Contains rows with the same uniques but different any other column values</returns>
        public List<DataTableDiff> TableGetDiffRows(DataTable whatTable, DataTable withTable)
        {
            var diffs = new List<DataTableDiff>();

            foreach (var war in whatTable.Rows)
            {
                var wir = TableFindSimmilarRow(war, withTable);
                if (wir != null)
                {
                    if (!RowCompareDataRows(war, wir))
                        diffs.Add(new DataTableDiff { ExpectRow = war, ActualRow = wir });
                }
            }
            return diffs;
        }

        /// <summary>
        /// Get list of Missed DataRows. DataRows from whatTable that don't present in withTable (just by unique values)
        /// </summary>
        /// <param name="whatTable">What DataTable</param>
        /// <param name="withTable">With what DataTable</param>
        /// <returns>List of Missed DataRows</returns>
        public List<DataRow> TableGetMissedRows(DataTable whatTable, DataTable withTable)
        {
            var missed = new List<DataRow>();

            foreach (var war in whatTable.Rows)
            {
                if (TableFindSimmilarRow(war, withTable) == null)
                    missed.Add(war);
            }

            return missed;
        }

        /// <summary>
        /// Get list of Extra DataRows. DataRows from withTable that don't present in wathTable (just by unique values)
        /// </summary>
        /// <param name="whatTable">What DataTable</param>
        /// <param name="withTable">With what DataTable</param>
        /// <returns>List of Extra DataRows</returns>
        public List<DataRow> TableGetExtraRows(DataTable whatTable, DataTable withTable)
        {
            var extra = new List<DataRow>();

            foreach (var wir in withTable.Rows)
            {
                if (TableFindSimmilarRow(wir, whatTable) == null)
                    extra.Add(wir);
            }

            return extra;
        }

        /// <summary>
        /// Get dictionary of DataRow values for specified columns in string representation
        /// </summary>
        /// <param name="columnNames">List of column names</param>
        /// <param name="whereRow">From what row</param>
        /// <returns>Dictionary of DataRow values for specified columns in string representation</returns>
        public Dictionary<string, string> RowGetColumnValues(List<string> columnNames, DataRow whereRow)
        {
            var vals = new Dictionary<string, string>();

            foreach (var column in columnNames)
            {
                vals.Add(column, whereRow[column, true]);
            }
            return vals;
        }
    }
}
