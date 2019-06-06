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

            var promoUrl = promoStandardList.Where(p => !string.IsNullOrEmpty(p.LiveUrl)).Select(p => p.LiveUrl.Trim()).Distinct();
            var url_count = promoUrl.Count();

            var supplierURL = existingSupplier.Configuration.SelectMany(e => e.Services.Where(e1 => !string.IsNullOrEmpty(e1.Value.Url)).Select(e2 => e2.Value.Url.Trim())).Distinct();
            var s_count = supplierURL.Count();

            supplierURL = supplierURL.Where(s => !s.Contains("ftp") && !s.Contains("dummy"));
            var s_count0 = supplierURL.Count();

            //supplierURL = supplierURL.Where(s => !s.Contains("dummy"));
            //var s_count1 = supplierURL.Count();
            //var l1 = supplierURL.Where(s => s.Contains("ftp"));
            //var c1 = l1.Count();
            //var l2 = supplierURL.Where(s => s.Contains("dummy"));
            //var c2 = l2.Count();

            var res = promoUrl.Except(supplierURL);
            var r_count = res.Count();

            TextWriter txt = new StreamWriter("D:\\promo.csv");
            foreach (var p in promoUrl)
            {
                txt.WriteLine(p);
            }
            txt.Close();

            TextWriter txt1 = new StreamWriter("D:\\supplier.csv");
            foreach (var s in supplierURL)
            {
                txt1.WriteLine(s);
            }
            txt1.Close();

            TextWriter txt2 = new StreamWriter("D:\\result.csv");
            foreach (var r in res)
            {
                txt2.WriteLine(r);
            }
            txt2.Close();


            int cnt = 0;
            foreach (var promo in promoUrl)
            {
                foreach (var supUrl in supplierURL)
                {
                    if (supUrl.Equals(promo, StringComparison.OrdinalIgnoreCase))
                    {
                        cnt++;
                        break;
                    }
                }
            }
            Console.WriteLine("Press a key to carry on...");
            Console.ReadLine();
        }
    }
}
