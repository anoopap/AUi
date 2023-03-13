using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public static class General
    {
        public static Dictionary<string,object> Settings(string path,string sheet)
        {
            DataTable dt;
            dt = Excel.ReadRange(path, sheet);
            Dictionary<string, object> settingsDictionary = new Dictionary<string, object>();
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row["Name"].ToString()))
                {
                }
                else
                {
                    settingsDictionary.Add(row["Name"].ToString().Trim(), row["Value"].ToString().Trim());                    
                }
            }
            return settingsDictionary;
        }

        public static void KillApplications(string applicationName)
        {
            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.Equals(applicationName))
                    {
                        clsProcess.Kill();
                    }
                }
            }
            catch(Exception e)
            {

            }
            
        }

        public static void WaitForDownloadsToComplete(string filePath,int timeout=30)
        {
           
            DateTime DateTimeVar = DateTime.Now;
            bool FindFileFlag = true;
            while (FindFileFlag)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        FindFileFlag = false;
                    }
                    else
                    {
                        throw new SystemException("File not found");
                    }
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

        public static void WaitForDownloadsToCompleteByPath(string folderPath, int timeout = 30)
        {

            DateTime DateTimeVar = DateTime.Now;
            bool FindFileFlag = true;
            while (FindFileFlag)
            {
                try
                {
                    if (Directory.GetFiles(folderPath).Count() > 0)
                    {
                        FindFileFlag = false;
                    }
                    else
                    {
                        throw new SystemException("File not found");
                    }
                }
                catch (Exception ex)
                {
                    //Check timeout exceeds.
                    if (DateTime.Now > DateTimeVar.AddSeconds(timeout))
                    {
                        throw (ex);
                    }
                }
            }
        }

    }
}
