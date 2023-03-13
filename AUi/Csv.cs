using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public static class Csv
    {
        public static DataTable ReadCSV(string file, string delemiter = ",", string[] headers = null, int skip = 0)
        {
            var lines = File.ReadLines(file);
            string data = null;
            foreach (string line in lines.Skip(skip))
            {
                data = data + line + Environment.NewLine;
            }
            DataTable dataTable = new DataTable();
            if (headers != null)
            {
                foreach (var header in headers)
                    dataTable.Columns.Add(header);
            }
            else
            {
                string first = new StringReader(data).ReadLine();
                string[] seperator = { delemiter };
                foreach (var hed in first.Split(seperator, StringSplitOptions.RemoveEmptyEntries))
                {
                    dataTable.Columns.Add(hed.Replace("\"", "").Trim());
                }

            }
            string[] linesTemp = data.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
            data = string.Join(Environment.NewLine, linesTemp);

            using (var reader = new StringReader(data))
            {
                TextFieldParser csvParser = new TextFieldParser(reader) { HasFieldsEnclosedInQuotes = true, Delimiters = new string[] { delemiter } };

                while (!csvParser.EndOfData)
                {
                    var dataTableRow = dataTable.NewRow();
                    dataTableRow.ItemArray = csvParser.ReadFields();
                    dataTable.Rows.Add(dataTableRow);
                }
            }
            return dataTable;
        }
    }
}
