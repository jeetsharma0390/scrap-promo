using System;
using System.Collections.Generic;
using System.Linq;
using ExtractImplementation.Model;
using HtmlAgilityPack;

namespace ExtractImplementation.Services
{
    public class PromostandardService
    {
        public List<Promostandard> GetPromostandardList()
        {            
            var promoStandardList = new List<Promostandard>();
            var promoUrl = "https://promostandards.org/endpoint/overview/?page=";
            var pageNum = 1;

            HtmlWeb web = new HtmlWeb();
            Console.WriteLine($"ExtractImplementation - Start - Get Promostandard data from page {pageNum}.");
            HtmlDocument doc = web.Load(promoUrl + pageNum.ToString());
            var rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']/tr");
            while (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("./td");
                    if (cells != null)
                    {
                        if (cells[2].InnerText.Trim() == "Supplier")
                        {
                            var promoStandard = new Promostandard();
                            promoStandard.CompanyName = cells[1].InnerText.Trim();
                            promoStandard.Type = cells[2].InnerText.Trim();
                            promoStandard.Service = cells[3].InnerText.Trim();
                            promoStandard.Version = cells[4].InnerText.Trim();
                            promoStandard.Status = cells[5].InnerText.Trim();
                            promoStandard.LiveUrl = cells[7].SelectNodes("./a[@href]").FirstOrDefault().Attributes["href"].Value.Trim();
                            promoStandard.TestUrl = cells[9].SelectNodes("./a[@href]")?.FirstOrDefault().Attributes["href"].Value.Trim();
                            promoStandardList.Add(promoStandard);
                        }
                    }
                }
                Console.WriteLine($"ExtractImplementation - End - Get Promostandard data from page {pageNum}, count = {rows.Count - 1}");
                pageNum++;
                Console.WriteLine($"ExtractImplementation - Start - Get Promostandard data from page {pageNum}.");
                doc = web.Load(promoUrl + pageNum.ToString());
                rows = doc.DocumentNode.SelectNodes("//table[@class='table table-striped']/tr");
                if (rows == null)
                    Console.WriteLine($"No data on page {pageNum}");

            }
            return promoStandardList;
        }
    }
}
