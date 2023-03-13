using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AUi
{
    public class GDrive
    {
        private static string pathToServiceAccountKeyFile;  // field
        public static string PathToServiceAccountKeyFile    // property
        {
            get { return pathToServiceAccountKeyFile; }
            set { pathToServiceAccountKeyFile = value; }
        }

        private static string serviceAccountEmail;
        public static string ServiceAccountEmail    // property
        {
            get { return serviceAccountEmail; }
            set { serviceAccountEmail = value; }
        }

        static string[] scopes = { SheetsService.Scope.Spreadsheets, DriveService.ScopeConstants.Drive };
        public static Folder FolderExists(string Parentid, string FolderName)
        {
            Folder fe = new Folder();
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
             .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var listitems = service.Files.List();
            listitems.Q = "parents in '" + Parentid + "' and mimeType = 'application/vnd.google-apps.folder'";
            var result = listitems.Execute();
            fe.FolderFound = false;
            fe.FolderID = "";
            foreach (var file in result.Files)
            {
                if (FolderName == file.Name)
                {                  
                    fe.FolderFound = true;
                    fe.FolderID = file.Id;
                }
            }
            return (fe);
        }

        public static GFile SearchFile(string Parentid, string FileName)
        {
            GFile fe = new GFile();
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
             .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var listitems = service.Files.List();
            listitems.PageSize = 1000;
            listitems.Q = $"'{Parentid}' in parents and trashed = false";
            var result = listitems.Execute();
            foreach (var file in result.Files)
            {
                if (FileName == Path.GetFileNameWithoutExtension(file.Name))
                {
                    fe.FileID = file.Id;
                    fe.FileName = file.Name;
                    fe.Extension = file.FileExtension;
                    fe.Webview = file.WebViewLink;
                    fe.Webview = file.WebContentLink;
                }
            }
            return (fe);
        }
        public static GFile FindFileByID(string Parentid)
        {
            GFile fe = new GFile();
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
             .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var listitems = service.Files.List();
            listitems.PageSize = 1000;
            listitems.Q = $"'{Parentid}' in parents and trashed = false";
            var result = listitems.Execute();
           
            foreach (var file in result.Files)
            {
                fe.FileID = Parentid;
                fe.FileName = file.Name;
                fe.Webview = file.WebContentLink;
                fe.WebViewLink = file.WebViewLink;
                fe.Extension = file.FileExtension;
            }
            return (fe);
        }
        public static GFile UploadasGoogleSheet(string Parentid, string filepath,string UploadFileName="")
        {
            if (UploadFileName == "") 
            {
                UploadFileName = Path.GetFileNameWithoutExtension(filepath);
            }
            

            //credentail and service configuration
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(filepath));

            //creating and upload sheet - file name will contain extension
            var sheetsFile = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "File.xlsx",
                Parents = new List<String> { Parentid },
                MimeType = "application/vnd.google-apps.spreadsheet"
            };

            var request = service.Files.Create(sheetsFile, memoryStream, sheetsFile.MimeType);
            request.SupportsAllDrives = true;
            request.Upload();

            //removing extension from file name using rename (update) file request
            
            string SheetId = request.ResponseBody?.Id.ToString();
            var req = service.Files.Get(SheetId);
            req.SupportsAllDrives = true;
            Google.Apis.Drive.v3.Data.File file =req.Execute();
            file.Id = null;
            file.Name = UploadFileName;
            FilesResource.UpdateRequest request1 = service.Files.Update(file, SheetId);
            request1.SupportsAllDrives = true;
            request1.Execute();
            GFile filedata = new GFile();
            filedata.WebViewLink = request.ResponseBody?.WebViewLink;
            filedata.FileID = request.ResponseBody?.Id;
            filedata.Webview = request.ResponseBody?.WebContentLink;
            return (filedata);

        }






        public static GFile CopyGoogleFile(string source_fileid, string destination_folderid, string destination_filename = "")
        {
            string UploadFileName;
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
         .CreateScoped(scopes);

            //get File Data
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential

            });

            var request = service.Files.Get(source_fileid);
            var mimetype = request.Execute().MimeType;
            var filename = request.Execute().Name;

            if (destination_filename == "")
            {
                UploadFileName = filename;
            }
            else
            {
                UploadFileName = destination_filename;
            }


            //create metadata
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = UploadFileName,
                Parents = new List<String> { destination_folderid },
                MimeType = mimetype
            };

            var request2 = service.Files.Copy(fileMetadata, source_fileid);
            request2.Fields = "id";
            var result = request2.Execute();
            GFile filedata = new GFile();
            filedata.WebViewLink = result.WebViewLink;
            filedata.FileID = result.Id;
            filedata.Webview = result.WebContentLink;
            return (filedata);
        }


        public static void SaveStreamAsFile(string filePath, MemoryStream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);

            using (System.IO.FileStream outputFileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                inputStream.WriteTo(outputFileStream);
            }
        }


        public static string CreateFolder(string Parentid, string FolderName)
        {
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
             .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = FolderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>
                    {
                        Parentid
                    }
            };

            // Create a new folder on drive.
            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            var result = request.Execute();
            return (result.Id);
        }



        //upload file
        public static GFile UploadFile(string filepath, string DirectoryId,string UploadFileName = null,bool supportSharedDrive=false)
        {

           if( string.IsNullOrEmpty(UploadFileName))
              {
                UploadFileName = Path.GetFileNameWithoutExtension(filepath);
            }
            
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(scopes);
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = UploadFileName,
                Parents = new List<String> { DirectoryId },
                MimeType = MimeMapping.GetMimeMapping(filepath)
            };
            var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(filepath));
            FilesResource.CreateMediaUpload request;
            request = service.Files.Create(fileMetadata, memoryStream, fileMetadata.MimeType);
            request.SupportsAllDrives = supportSharedDrive;
            request.Fields = "*";
            request.Upload();
            GFile gfiledata = new GFile();
            gfiledata.WebViewLink = request.ResponseBody?.WebViewLink;
            gfiledata.FileID = request.ResponseBody?.Id;
            gfiledata.Webview = request.ResponseBody?.WebContentLink;
            return gfiledata;
        }
       


        //Download file
        public static MemoryStream DriveDownloadFile(String fileId, String path)
        {
            try
            {


                var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
            .CreateScoped(scopes);
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential

                });

                var request = service.Files.Get(fileId);
                request.SupportsAllDrives = true;
                var mimetype = request.Execute().MimeType;
                var filename = request.Execute().Name;
                MemoryStream stream1 = new MemoryStream();

                if (mimetype == "application/vnd.google-apps.spreadsheet")
                {

                    var request1 = service.Files.Export(fileId, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    // Add a handler which will be notified on progress changes.
                    // It will notify on each chunk download and when the
                    // download is completed or failed.
                    request1.MediaDownloader.ProgressChanged +=
                        progress =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        Console.WriteLine("Download complete.");
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        Console.WriteLine("Download failed.");
                                        break;
                                    }
                            }
                        };
                    
                    request1.Download(stream1);

                    SaveStreamAsFile(path, stream1, filename + ".xlsx");

                }



                else if (mimetype == ("application/vnd.google-apps.document"))
                {
                    var request1 = service.Files.Export(fileId, "application/pdf");


                    request1.MediaDownloader.ProgressChanged +=
                        progress =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        Console.WriteLine("Download complete.");
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        Console.WriteLine("Download failed.");
                                        break;
                                    }
                            }
                        };

                    request1.Download(stream1);

                    SaveStreamAsFile(path, stream1, filename + ".zip");
                }



                else

                {
                    var request1 = service.Files.Get(fileId);


                    request1.MediaDownloader.ProgressChanged +=
                        progress =>
                        {
                            switch (progress.Status)
                            {
                                case DownloadStatus.Downloading:
                                    {
                                        Console.WriteLine(progress.BytesDownloaded);
                                        break;
                                    }
                                case DownloadStatus.Completed:
                                    {
                                        Console.WriteLine("Download complete.");
                                        break;
                                    }
                                case DownloadStatus.Failed:
                                    {
                                        Console.WriteLine("Download failed.");
                                        break;
                                    }
                            }
                        };

                    request1.Download(stream1);

                    SaveStreamAsFile(path, stream1, filename);

                }


            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }
        private static DriveService Authentication()
        {
            UserCredential credential;
            string[] Scopes = { DriveService.Scope.Drive };
            string ApplicationName = "GoogleDrive";
            using (var stream =
                new FileStream(PathToServiceAccountKeyFile, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        public static void GivePermission(string fileid, string mailid, string access_type = "reader", bool send_notification = false, string permission_type = "user")
        {
            Permission perms = new Permission();
            perms.Role = access_type;
            perms.Type = permission_type;
            perms.EmailAddress = mailid;
            DriveService service = Authentication();
            var req = service.Permissions.Create(perms, fileid);
            req.SendNotificationEmail = send_notification;
            req.Execute();

        }


    }




    public class GFile
    {
        public string WebViewLink { get; set; }
        public string FileID { get; set; }
        public string Webview { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
    public class Folder
    {
       
        public string FolderID { get; set; }
        public bool FolderFound { get; set; }
    }
}

