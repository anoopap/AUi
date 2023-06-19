using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


//Logs option

namespace AUi
{
    public static class Logs
    {
        private static string connection_setuppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Connection_Setup.cba";


        public static void Addlog(Logdata logdata)
        {
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //Console.WriteLine(content.ReadAsStringAsync().Result);

                    var json = JsonConvert.SerializeObject(logdata);
                    //var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");



                    var content = new StringContent(json.ToString());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.ContentEncoding.Clear();
                    content.Headers.ContentEncoding.Add(Encoding.UTF8.WebName);

                    //Debug.WriteLine(json);
                    string serializedContent = content.ReadAsStringAsync().Result;
                    Console.WriteLine(serializedContent);


                    HttpResponseMessage response = Task.Run(() => client.PostAsync("http://gmgc-1671//Jobs/Addlogs", content)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync();

                    }
                    else
                    {
                        throw new SystemException("Request failed");
                    }


                }
            }
            catch(Exception ex) { }
        }

        public static void addlogs(int jobid = -1, string logMessage = "", string logsource = "Main", string logtype = "Information")
        {
            try
            {
                if (jobid >= 0)
                {
                    Logdata log = new Logdata();
                    log.job_id = jobid;
                    log.log_message = logMessage;
                    log.log_source = logsource;
                    log.log_type = logtype;
                    Addlog(log);
                }
            }
            catch(Exception ex) { }
        }









    }
    public class Logdata
    {
        public int job_id { get; set; }
        public string log_message { get; set; }
        [JsonProperty("log_source")]
        public string log_source { get; set; }
        [JsonProperty("log_type")]
        public string log_type { get; set; }
        public string local_machine_key { get; set; }

    }








}



