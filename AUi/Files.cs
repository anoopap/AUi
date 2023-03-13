using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AUi
{
    public class Files
    {
        public static string ReadFile(string FilePath)
        {
            string text = System.IO.File.ReadAllText(FilePath);
            return text;
        }
        public static void WriteFile(string FilePath, string Text)
        {
            File.WriteAllText(FilePath, Text);
        }
        public static void AppendFile(string FilePath, string Text)
        {
            File.AppendAllText(FilePath, Text);
        }
        public static void CopyFile(string sourcePath, string targetPath, string fileName, Boolean overwrite = false)
        {
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.File.Copy(sourceFile, destFile, overwrite);
        }
        public static void CopyFileFullPath(string SourceFullPath, string TargetFullPath, Boolean overwrite = false)
        {
            try
            {
                char temp = '/';
                string sourceName = SourceFullPath.Substring(SourceFullPath.LastIndexOf(temp) + 1);
                string destName = TargetFullPath.Substring(TargetFullPath.LastIndexOf(temp) + 1);
                string fileName = sourceName;
                string sourcePath = SourceFullPath.Substring(0, SourceFullPath.LastIndexOf(temp));
                string targetPath = TargetFullPath.Substring(0, SourceFullPath.LastIndexOf(temp));
                if (sourceName != destName)
                {
                    Console.WriteLine("Provide Full Path, Use Copy File Instead");
                }
                else
                {
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.Directory.CreateDirectory(targetPath);
                    System.IO.File.Copy(sourceFile, destFile, overwrite);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Copy Failed " + ex.Message);
            }
        }
        public static void DeleteFile(string FilePath)
        {
            System.IO.File.Delete(FilePath);
        }
        public static void MoveFile(string sourceFullPath, string destFullPath)
        {
            try
            {
                if (!File.Exists(sourceFullPath))
                {
                    // This statement ensures that the file is created,  
                    // but the handle is not kept.  
                    using (FileStream fs = File.Create(sourceFullPath)) { }
                }
                // Ensure that the target does not exist.  
                if (File.Exists(destFullPath))
                    File.Delete(destFullPath);
                // Move the file.  
                File.Move(sourceFullPath, destFullPath);
                Console.WriteLine("{0} was moved to {1}.", sourceFullPath, destFullPath);

                // See if the original exists now.  
                if (File.Exists(sourceFullPath))
                {
                    Console.WriteLine("The original file still exists, which is unexpected.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        public static Boolean FolderExists(string FilePath)
        {
            Boolean Flag = false;
            if (FolderExists(FilePath))
            {
                Flag = true;
            }
            return Flag;
        }
        public static void CreateFolder(string FolderLocation, string FolderName)
        {
            FolderLocation = FolderLocation + FolderName;
            System.IO.Directory.CreateDirectory(FolderLocation);
        }
    }
}
