using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public class Workbook
    {
        public static System.Data.DataTable ReadExcel(string filepath, string sheetname = "", int startrow = 0)
        {
            IWorkbook workbook;
            if (filepath.Contains(".xlsx"))
            {
                using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(stream); // XSSFWorkbook for XLSX
                }
            }
            else if (filepath.Contains(".xls"))
            {
                using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(stream); // XSSFWorkbook for XLS
                }
            }
            else
            {
                throw new Exception("Invalid File Format");
            }
            ISheet sheet;
            var dataTable = new System.Data.DataTable();
            if (sheetname == "")
            {
                sheet = workbook.GetSheetAt(0);
                dataTable = new System.Data.DataTable(sheet.SheetName);
            }
            else
            {
                sheet = workbook.GetSheet(sheetname);
                dataTable = new System.Data.DataTable(sheetname);
            }
            // write the header row
            var headerRow = sheet.GetRow(startrow);
            foreach (var headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.ToString());
            }

            // write the rest
            for (int i = startrow + 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var sheetRow = sheet.GetRow(i);
                var dtRow = dataTable.NewRow();
                dtRow.ItemArray = dataTable.Columns
                    .Cast<System.Data.DataColumn>()
                    .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString())
                    .ToArray();
                dataTable.Rows.Add(dtRow);
            }
            return dataTable;
        }
        public static void WriteExcel(System.Data.DataTable dt, string filepath, string sheetname = "Sheet1")
        {
            if (System.IO.File.Exists(filepath))
            {
                IWorkbook wb;
                if (filepath.Contains(".xlsx"))
                {
                    using (var stream1 = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        wb = new XSSFWorkbook(stream1); // XSSFWorkbook for XLSX
                    }
                }
                else if (filepath.Contains(".xls"))
                {
                    using (var stream1 = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        wb = new HSSFWorkbook(stream1); // XSSFWorkbook for XLS
                    }
                }
                else
                {
                    throw new Exception("Invalid File Format");
                }
                using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    ISheet sheet;

                    //Get List of Sheets
                    List<string> oList = GetSheets(wb);
                    if (oList.Contains(sheetname.ToUpper())) { sheet = wb.GetSheet(sheetname); } else { sheet = wb.CreateSheet(sheetname); }

                    ICreationHelper cH = wb.GetCreationHelper();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            cell.SetCellValue(cH.CreateRichTextString(dt.Rows[i].ItemArray[j].ToString()));
                        }
                    }
                    wb.Write(stream);
                }
            }
            else
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Append))
                {
                    IWorkbook wb = new XSSFWorkbook();
                    ISheet sheet;

                    //Get List of Sheets
                    List<string> oList = GetSheets(wb);
                    if (oList.Contains(sheetname.ToUpper())) { sheet = wb.GetSheet(sheetname); } else { sheet = wb.CreateSheet(sheetname); }

                    ICreationHelper cH = wb.GetCreationHelper();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = sheet.CreateRow(i);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            cell.SetCellValue(cH.CreateRichTextString(dt.Rows[i].ItemArray[j].ToString()));
                        }
                    }
                    wb.Write(stream);
                }
            }

        }
        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static DataRow GetHeaders(DataTable dt)
        {
            DataRow row = dt.NewRow();
            DataColumnCollection columns = dt.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                row[i] = columns[i].ColumnName;
            }
            return row;
        }
        public static List<string> GetSheets(IWorkbook wb)
        {
            List<string> oList = new List<string>();
            for (int i = 0; i < wb.NumberOfSheets; i++)
            {
                string SheetName = wb.GetSheetName(i).Trim().Replace(" ", "").ToUpper();

                if (!string.IsNullOrEmpty(SheetName))
                {
                    oList.Add(SheetName);
                }
            }
            return (oList);
        }
        public static DataTable ReadSheets(string filepath)
        {
            //Read Sheets in a foler
            string[] requiredcolumns = { "Log Date", "Direction", "Employee Code", "Employee Name", "Company", "Department" };
            string[] filesinpath = Directory.GetFiles(filepath, "*.xls");
            System.Data.DataTable dt_in = new DataTable();
            System.Data.DataTable dt_temp;
            foreach (string item in filesinpath)
            {
                dt_temp = ReadExcel(item, "", 7);
                dt_in.Merge(dt_temp);
            }

            System.Data.DataView view = new System.Data.DataView(dt_in);
            System.Data.DataTable dt_merged =
                    view.ToTable("Selected", false, requiredcolumns);
            dt_merged = dt_merged.Rows
    .Cast<DataRow>()
    .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                     string.IsNullOrWhiteSpace(field as string)))
    .CopyToDataTable();

            return dt_merged;
        }

        public static DataTable Check(DataTable dt)
        {
            int indexer = 0;
            DataTable out_dt = new DataTable();
            out_dt.Columns.Add("Direction", typeof(string));
            out_dt.Columns.Add("Employee Code", typeof(string));
            out_dt.Columns.Add("Employee Name", typeof(string));
            out_dt.Columns.Add("Company", typeof(string));
            out_dt.Columns.Add("Department", typeof(string));
            out_dt.Columns.Add("Log Date", typeof(DateTime));
            DateTime timer = DateTime.Parse(dt.Rows[0][5].ToString().Trim());
            string temp = dt.Rows[0][0].ToString().Trim().ToLower();
            out_dt = dt.Rows
                    .Cast<DataRow>()
                    .Where(row => row == dt.Rows[0])
                    .CopyToDataTable();
            //out_dt.Rows.Add(dt.Rows[0]);
            foreach (DataRow row in dt.Rows)
            {
                if (indexer == 0)
                {
                    indexer = indexer + 1;
                    continue;
                }
                if (temp == row[0].ToString().Trim().ToLower())
                {
                    if (TimeSpan.Compare(DateTime.Parse(row[5].ToString().Trim().ToLower()) - timer, new TimeSpan(00, 00, 10)) != 1)
                    {
                        indexer = indexer + 1;
                        continue;
                    }
                    else
                    {
                        out_dt.Rows.Add(row.ItemArray);
                    }
                }
                else
                {
                    out_dt.Rows.Add(row.ItemArray);
                    //out_dt.Rows.Add(row);
                    temp = row[0].ToString().Trim().ToLower();
                    timer = DateTime.Parse(row[5].ToString().Trim());
                }
                indexer = indexer + 1;
            }
            return out_dt;
        }
    }
}
