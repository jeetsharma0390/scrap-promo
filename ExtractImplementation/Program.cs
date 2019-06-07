using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExtractImplementation.Model;
using ExtractImplementation.Services;

namespace ExtractImplementation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ExtractImplementation - Start - Get records from promostandard site.");
            PromostandardService promoObj = new PromostandardService();
            var promoStandardList = promoObj.GetPromostandardList();
            Console.WriteLine($"ExtractImplementation - End - Get records from promostandard site, count = {promoStandardList.Count}");

            Console.WriteLine("ExtractImplementation - Start - Get data from Excit.");
            ExcitService excitObj = new ExcitService();
            var existingSupplier = excitObj.GetExcitImplementationList();
            Console.WriteLine($"ExtractImplementation - End  - Get data from Excit, count = {existingSupplier.SupplierAPIs.Count}");
            var promoUrl = promoStandardList.Where(promo => !string.IsNullOrEmpty(promo.LiveUrl)).Select(promo => promo.LiveUrl.Trim()).Distinct();
            var supplierUrl = existingSupplier.Configuration
                                                    .SelectMany(suppliers => suppliers.Services.Where(services => !string.IsNullOrEmpty(services.Value.Url))
                                                    .Select(service => service.Value.Url.Trim())).Distinct();
            Console.WriteLine("ExtractImplementation - Start  - Comparision between Promostandard and Supplier APIs.");
            var urlsNotImplemented = new List<string>();
            var urlsImplemented = new List<string>();
            foreach (var promo in promoUrl)
            {
                var flag = false;
                foreach (var supp in supplierUrl)
                {
                    if (supp.Equals(promo, StringComparison.OrdinalIgnoreCase))
                    {
                        flag = true;
                        urlsImplemented.Add(promo);
                        break;
                    }
                }
                if (flag == false)
                {
                    urlsNotImplemented.Add(promo);
                }
            }
            Console.WriteLine("ExtractImplementation - End  - Comparision between Promostandard and Supplier APIs.");


            var path = "E:\\ExcelFiles\\Promostandard.csv";
            if (File.Exists(path)) File.Delete(path);
            Console.WriteLine($"ExtractImplementation - Start - Making CSV file on path : { path}");
            TextWriter txt = new StreamWriter(path);
            var csvFile = "Company, " + "Type, " + "Service, " + "Version, " + "Status, " + "LiveURL, " + "ASI Status,";
            txt.WriteLine(csvFile);
            var unImplementedPromostandardList = promoStandardList.Where(p => urlsNotImplemented.Contains(p.LiveUrl)).Distinct().ToList();
            var unImplementedPromostandardOrderedList = unImplementedPromostandardList.OrderBy(promo => promo.CompanyName).ThenBy(promo => promo.Service).ThenBy(promo => promo.Version);
            foreach (var promostandard in unImplementedPromostandardOrderedList)
            {
                string row = string.Empty;
                row += promostandard.CompanyName.Replace(",", ";") + ',';
                row += promostandard.Type.Replace(",", ";") + ',';
                row += promostandard.Service.Replace(",", ";") + ',';
                row += promostandard.Version.Replace(",", ";") + ',';
                row += promostandard.Status.Replace(",", ";") + ',';
                row += promostandard.LiveUrl.Replace(",", ";") + ',';
                row += "Un-Implemented,";
                txt.WriteLine(row);
            }
            var implementedPromostandardList = promoStandardList.Where(p => urlsImplemented.Contains(p.LiveUrl)).Distinct().ToList();
            var implementedPromostandardOrderedList = implementedPromostandardList.OrderBy(promo => promo.CompanyName).ThenBy(promo => promo.Service).ThenBy(promo => promo.Version);
            foreach (var implemented in implementedPromostandardOrderedList)
            {
                string row = string.Empty;
                row += implemented.CompanyName.Replace(",", ";") + ',';
                row += implemented.Type.Replace(",", ";") + ',';
                row += implemented.Service.Replace(",", ";") + ',';
                row += implemented.Version.Replace(",", ";") + ',';
                row += implemented.Status.Replace(",", ";") + ',';
                row += implemented.LiveUrl.Replace(",", ";") + ',';
                row += "Implemented,";
                txt.WriteLine(row);
            }
            txt.Close();
            Console.WriteLine($"ExtractImplementation - End - Making CSV file on path : { path}");
            Console.WriteLine("Press a key to carry on...");
            Console.ReadLine();
        }
    }
}
