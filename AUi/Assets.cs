using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AUi
{
    public class Assets
    {
        private static string connection_setuppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Connection_Setup.cba";

        private class Asset
        {
            public string asset_name { get; set; }
            public string asset_value { get; set; }
            public string asset_username { get; set; }
            public string asset_password { get; set; }
        }

        public class Credentials
        {            
            public string asset_username { get; set; }
            public string asset_password { get; set; }
        }
        public class AssetValue
        {
            public string asset_value { get; set; }
        }
        public static AssetValue GetAsset(string asset_name)
        {
            AssetValue av = new AssetValue();
            Asset asset = new Asset();
            if (IsConnectionValid())
            {
                DateTime DateTimeVar = DateTime.Now;
                bool successFlag = true;
                
                while (successFlag)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            string key = Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[0];
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("asset_name", asset_name);
                            dict.Add("local_machine_key", key);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            var content = new StringContent(JsonConvert.SerializeObject(dict).ToString(), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = Task.Run(() => client.PostAsync(Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[1]+ "/Assets/GetAssets", content)).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var data = response.Content.ReadAsStringAsync();
                                dynamic dynamicVar = JsonConvert.DeserializeObject(data.Result.ToString());
                                asset = JsonConvert.DeserializeObject<Asset>(dynamicVar);
                                av.asset_value = asset.asset_value;
                            }
                            else
                            {
                                throw new Exception("GetAsset failed with status code "+response.StatusCode.ToString());
                            }

                        }
                        successFlag = false;
                    }
                    catch (Exception ex)
                    {
                        //Check timeout exceeds.
                        if (DateTime.Now > DateTimeVar.AddSeconds(30))
                        {
                            throw (ex);
                        }
                    }
                }
            }
            

            
            return av;
        }

        public static Credentials GetCredentials(string asset_name)
        {
            Credentials cd = new Credentials();
            Asset asset = new Asset();
            if (IsConnectionValid())
            {
                DateTime DateTimeVar = DateTime.Now;
                bool successFlag = true;

                while (successFlag)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            string key = Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[0];
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("asset_name", asset_name);
                            dict.Add("local_machine_key", key);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            var content = new StringContent(JsonConvert.SerializeObject(dict).ToString(), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = Task.Run(() => client.PostAsync(Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[1] + "/Assets/GetCredential", content)).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var data = response.Content.ReadAsStringAsync();
                                dynamic dynamicVar = JsonConvert.DeserializeObject(data.Result.ToString());
                                asset = JsonConvert.DeserializeObject<Asset>(dynamicVar);
                                cd.asset_password = asset.asset_password;
                                cd.asset_username = asset.asset_username;
                            }
                            else
                            {
                                throw new Exception("GetCredentials failed with status code " + response.StatusCode.ToString());
                            }

                        }
                        successFlag = false;
                    }
                    catch (Exception ex)
                    {
                        //Check timeout exceeds.
                        if (DateTime.Now > DateTimeVar.AddSeconds(30))
                        {
                            throw (ex);
                        }
                    }
                }
            }

            return cd;
        }

        public static bool IsConnectionValid()
        {

            bool valid = false;
            if (File.Exists(connection_setuppath) )
            {
                string key = Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[0];
                using (HttpClient client = new HttpClient())
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("local_machine_key", key);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonConvert.SerializeObject(dict).ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = Task.Run(() => client.PostAsync(Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[1] + "/Machines/ConnectMachine", content)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync();

                        string result = JsonConvert.DeserializeObject(data.Result).ToString();
                        Dictionary<string, string> json = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                        string status = json["status"].ToString();
                        if (status == "connected")
                        {
                            valid = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Not a valid connection. Please check web platform connection.");
                    }
                }
            }
           
            return valid;
        }
    }
}
