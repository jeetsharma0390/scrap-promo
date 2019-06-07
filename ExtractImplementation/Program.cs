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
                        break;
                    }
                }
                if (flag == false)
                {
                    urlsNotImplemented.Add((promo));
                }
            }

            TextWriter txt = new StreamWriter("E:\\ExcelFiles\\Promostandard.csv");

            //Add the Header row for CSV file.
            string csvFile = "Company, " + "Type, " + "Service, " + "Version, " + "Status, " + "LiveURL, " + "ASI Action,";
            txt.WriteLine(csvFile);

            var finalResult = promoStandardList.Where(p => urlsNotImplemented.Contains(p.LiveUrl)).Distinct().ToList();

            foreach (var promostandard in finalResult)
            {
                string row = string.Empty;
                row += promostandard.CompanyName.ToString().Replace(",", ";") + ',';
                row += promostandard.Type.ToString().Replace(",", ";") + ',';
                row += promostandard.Service.ToString().Replace(",", ";") + ',';
                row += promostandard.Version.ToString().Replace(",", ";") + ',';
                row += promostandard.Status.ToString().Replace(",", ";") + ',';
                row += promostandard.LiveUrl.ToString().Replace(",", ";") + ',';
                row += "Implemented,";
                txt.WriteLine(row);
            }
            txt.Close();

            Console.WriteLine("Press a key to carry on...");
            Console.ReadLine();
        }
    }
}
