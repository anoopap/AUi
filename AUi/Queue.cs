using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AUi;
using Newtonsoft.Json;

namespace AUi
{
    public static class Queue
    {
        private static string connection_setuppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Connection_Setup.cba";
        public static QueueData GetQueueItems(int process_id)
        {
            
            DateTime DateTimeVar = DateTime.Now;
            bool successFlag = true;
            QueueData record = new QueueData();
            if (IsConnectionValid())
            {
                string key = Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[0];
                while (successFlag)
                {
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("process_id", process_id);
                            dict.Add("local_machine_key", key);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            var content = new StringContent(JsonConvert.SerializeObject(dict).ToString(), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = Task.Run(() => client.PostAsync(Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[1] + "/Queues/GetQueueItems", content)).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var data = response.Content.ReadAsStringAsync();
                                dynamic dynamicVar = JsonConvert.DeserializeObject(data.Result.ToString());
                                ProcessesRecords output = JsonConvert.DeserializeObject<ProcessesRecords>(dynamicVar);
                                record.record_id = output.record_id;
                                record.process_id = output.carebot_process_id;
                                record.data = output.carebot_data;
                                record.status = output.carebot_record_status;

                            }
                            else
                            {
                                throw new System.Exception("not valid");
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
           

            
            return record;
        }

        public static void  UpdateQueueItems(QueueData queueData)
        {
            string key = Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[0];
            queueData.local_machine_key = key;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(queueData).ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = Task.Run(() => client.PostAsync(Encryption.Decrypt(File.ReadAllText(connection_setuppath), Environment.MachineName).Split('#')[1] + "/Queues/UpdateQueueItems", content)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    
                }
                else
                {
                    throw new SystemException("Reques failed");
                }

            }
        }
        public static bool IsConnectionValid()
        {

            bool valid = false;
            if (File.Exists(connection_setuppath))
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
                }
            }

            return valid;
        }
        public class QueueData
        {
            public int process_id { get; set; }
            public string local_machine_key { get; set; }
            public int record_id { get; set; }
            public string status { get; set; }
            public string data { get; set; }
        }


        public class ProcessesRecords
        {
            public int record_id { get; set; }
            public int carebot_process_id { get; set; }
            public string carebot_data { get; set; }
            public string carebot_process_files { get; set; }
            public string carebot_record_status { get; set; }
            public string carebot_process_key { get; set; }
            public string carebot_manual_trigger { get; set; }
            public int carebot_current_retry { get; set; }
            public string carebot_process_machine { get; set; }
            public DateTime carebot_action_start_date { get; set; }
            public DateTime carebot_action_end_date { get; set; }
            public int carebot_priorty { get; set; }
        }

    }
}
