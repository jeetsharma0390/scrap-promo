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

            //var res = promoUrl.Except(supplierURL);
            //var r_count = res.Count();

            List<string> urlsNotImplemented = new List<string>();
            List<string> urlsImplemented = new List<string>();
            int count = 0;
            foreach (var promo in promoUrl)
            {
                var flag = false;
                foreach (var supp in supplierUrl)
                {
                    if (supp.Equals(promo, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
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

            TextWriter txt = new StreamWriter("E:\\ExcelFiles\\Promostandard-stage.csv");

            //Add the Header row for CSV file.
            string csvFile = "Company, " + "Type, " + "Service, " + "Version, " + "Status, " + "LiveURL, " + "ASI Status,";
            txt.WriteLine(csvFile);

            var unImplementedPromostandardList = promoStandardList.Where(p => urlsNotImplemented.Contains(p.LiveUrl)).Distinct().ToList();
            var unImplementedPromostandardOrderedList = unImplementedPromostandardList.OrderBy(promo => promo.CompanyName).ThenBy(promo => promo.Service).ThenBy(promo => promo.Version);
            foreach (var promostandard in unImplementedPromostandardOrderedList)
            {
                string row = string.Empty;
                row += promostandard.CompanyName.ToString().Replace(",", ";") + ',';
                row += promostandard.Type.ToString().Replace(",", ";") + ',';
                row += promostandard.Service.ToString().Replace(",", ";") + ',';
                row += promostandard.Version.ToString().Replace(",", ";") + ',';
                row += promostandard.Status.ToString().Replace(",", ";") + ',';
                row += promostandard.LiveUrl.ToString().Replace(",", ";") + ',';
                row += "UnImplemented,";
                txt.WriteLine(row);
            }


            var implementedPromostandardList = promoStandardList.Where(p => urlsImplemented.Contains(p.LiveUrl)).Distinct().ToList();
            var implementedPromostandardOrderedList = implementedPromostandardList.OrderBy(promo => promo.CompanyName).ThenBy(promo => promo.Service).ThenBy(promo => promo.Version);
            foreach (var implemented in implementedPromostandardOrderedList)
            {
                string row = string.Empty;
                row += implemented.CompanyName.ToString().Replace(",", ";") + ',';
                row += implemented.Type.ToString().Replace(",", ";") + ',';
                row += implemented.Service.ToString().Replace(",", ";") + ',';
                row += implemented.Version.ToString().Replace(",", ";") + ',';
                row += implemented.Status.ToString().Replace(",", ";") + ',';
                row += implemented.LiveUrl.ToString().Replace(",", ";") + ',';
                row += "Implemented,";
                txt.WriteLine(row);
            }
            txt.Close();

            Console.WriteLine("Press a key to carry on...");
            Console.ReadLine();
        }
    }
}
