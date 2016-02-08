namespace QA.TestLibs.Csv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Data;
    using CsvHelper;
    using System.IO;

    public class CsvManager
    {
        public DataTable GetDataTableFromCsv(string pathToCsvFile)
        {
            var dataTable = new DataTable();

            using (var csvReader = new CsvReader(new StreamReader(pathToCsvFile)))
            {
                while (csvReader.Read())
                {
                    var dr = new DataRow();

                    for (int i = 0; i < csvReader.FieldHeaders.Length; i++)
                    {
                        dr.Add(csvReader.FieldHeaders[i], csvReader.GetField(i));
                    }

                    dataTable.Add(dr);
                }
            }

            return dataTable;
        }

        public void SaveDataTableToCsv(DataTable dataTable, string pathToCsvFile)
        {
            using (var csvWriter = new CsvWriter(new StreamWriter(pathToCsvFile)))
            {
                var fr = dataTable.Rows.First();
                fr.Columns.ForEach(c => csvWriter.WriteField(c));
                csvWriter.NextRecord();

                dataTable.Rows.ForEach(
                    r =>
                    {
                        r.Values.ForEach(v => csvWriter.WriteField(v));
                        csvWriter.NextRecord();
                    }
                );
            }
        }
    }
}
