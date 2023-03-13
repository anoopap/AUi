using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public static class Excel
    {
        public static System.Data.DataTable ReadRange(string filePath, string sheetName,string password="")
        {
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            DataSet ds = new DataSet();
            try
            {
                objXL = new Microsoft.Office.Interop.Excel.Application();
                objWB = objXL.Workbooks.Open(filePath, ReadOnly: true, Password: password);//Your path to excel file.
                foreach (Microsoft.Office.Interop.Excel.Worksheet objSHT in objWB.Worksheets)
                {
                    if(objSHT.Name == sheetName)
                    {
                        int rows = objSHT.UsedRange.Rows.Count;
                        int cols = objSHT.UsedRange.Columns.Count;
                        System.Data.DataTable dt = new System.Data.DataTable();
                        int noofrow = 1;
                        //If 1st Row Contains unique Headers for datatable include this part else remove it
                        //Start
                        for (int c = 1; c <= cols; c++)
                        {
                            string colname = objSHT.Cells[1, c].Text;
                            dt.Columns.Add(colname);
                            noofrow = 2;
                        }
                        //END
                        for (int r = noofrow; r <= rows; r++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int c = 1; c <= cols; c++)
                            {
                                dr[c - 1] = objSHT.Cells[r, c].Text;
                            }
                            dt.Rows.Add(dr);
                        }
                        ds.Tables.Add(dt);
                    }
                    
                }
                //Closing workbook
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                return ds.Tables[0];
            }

            catch (Exception ex)
            {
                objWB.Saved = true;
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                //Response.Write("Illegal permission");
                return ds.Tables[0];
            }
        }

        public static string GetCellValue(string path, int row, int column)
        {
            Application excel = new Application();
            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(path);
            Worksheet excelSheet = wb.ActiveSheet;
            //Read the first cell
            string val = excelSheet.Cells[row, column].Value.ToString();
            wb.Close();
            return val;
        }

        public static void UpdateCell(string path, string text, int row, int column, string sheetName = "Sheet1")
        {

            Application excel = new Application();
            Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(path);
            Worksheet excelSheet = wb.Sheets[sheetName];
            //Read the first cell
            //string val = excelSheet.Cells[row, column].Value.ToString();
            excelSheet.Cells[row, column].Value = text;
            wb.Save();
            wb.Close();           
        }

        public static void WriteRange(System.Data.DataTable tbl, string excelFilePath ,string sheetName = "Sheet1", bool hearder = true)
        {
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                if (File.Exists(excelFilePath))
                {
                    Application excel = new Application();
                    Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(excelFilePath);
                    Worksheet excelSheet = wb.Sheets[sheetName];
                   
                    // column headings
                    if (hearder)
                    {
                        for (var i = 0; i < tbl.Columns.Count; i++)
                        {
                            excelSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                        }
                    }


                    // rows
                    for (var i = 0; i < tbl.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < tbl.Columns.Count; j++)
                        {
                            excelSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                        }
                    }

                    wb.Save();
                    wb.Close();
                }
                else
                {
                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();

                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    workSheet.Name = sheetName;

                    // column headings
                    if (hearder)
                    {
                        for (var i = 0; i < tbl.Columns.Count; i++)
                        {
                            workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                        }
                    }


                    // rows
                    for (var i = 0; i < tbl.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < tbl.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                        }
                    }

                    // check file path
                    if (!string.IsNullOrEmpty(excelFilePath))
                    {
                        try
                        {
                            workSheet.SaveAs(excelFilePath);
                            excelApp.Quit();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                                + ex.Message);
                        }
                    }
                    else
                    { // no file path is given
                        excelApp.Visible = false;
                    }
                }


               
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

    }


}
