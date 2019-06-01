using System;
using System.Collections.Generic;
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
            ExcitService excitObj=new ExcitService();
            var excistingSupplier = excitObj.GetExcitImplementationList();
            Console.WriteLine("ExtractImplementation - End - Get data from Excit.");

            Console.WriteLine("Press a key to carry on...");
            Console.ReadLine();
        }
    }
}
