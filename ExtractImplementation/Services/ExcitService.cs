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
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                var response = request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    suppliers = JsonConvert.DeserializeObject<List<Excit>>(streamReader.ReadToEnd());
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
