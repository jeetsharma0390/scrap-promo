using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ExtractImplementation.Model;

namespace ExtractImplementation.Services
{
    class ExcitService
    {
        public List<Excit> GetExcitImplementationList()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["EnvironmentUrl"];
            Console.WriteLine($"ExtractImplementation - Start - Get suppliers detail from \"{url}\".");
            var suppliers = new List<Excit>();
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(url);
                    
                    suppliers = JsonConvert.DeserializeObject<List<Excit>>(json);
                }
                Console.WriteLine($"ExtractImplementation - End - Get suppliers detail from \"{url}\".");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return suppliers;
           
        }
    }
}
