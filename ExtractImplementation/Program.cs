using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ExtractImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create("https://dc.stage-asicentral.com/v1/suppliers");
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            var response = request.GetResponse();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            var promoUrl = "https://promostandards.org/endpoint/overview/?page=";
            var pageNum = 1;

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(promoUrl + pageNum.ToString());
            var rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']/tr");
            do
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("./td");
                    if (cells != null)
                    {
                        var supplierName = cells[1].InnerText.Trim();
                    }
                }
                pageNum++;
                doc = web.Load(promoUrl + pageNum.ToString());
                rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']/tr");
            } while (rows != null);
        }
    }
}
