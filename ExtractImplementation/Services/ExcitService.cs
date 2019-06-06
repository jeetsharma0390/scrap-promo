using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ExtractImplementation.Model;
using ASI.Services.Excit;
using ASI.Contracts.Excit.Supplier.Version1;

namespace ExtractImplementation.Services
{
    class ExcitService
    {
        public Excit GetExcitImplementationList()
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["EnvironmentUrl"];
            ExcitClient excitClient = new ExcitClient(url, null, null, string.Empty);
            var excit = new Excit()
            {
                SupplierAPIs = new List<SupplierAPI>(),
                Configuration = new List<Configuration>()
            };
            Console.WriteLine($"ExtractImplementation - Start - Get suppliers detail from \"{url}\".");
            var supplierDetails = new List<SupplierAPI>();
            try
            {
                supplierDetails = excitClient.GetSuppliers().Result;
                excit.SupplierAPIs = supplierDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (var supplier in supplierDetails)
            {
                var configuration = excitClient.GetSupplierConfiguration(supplier.CompanyId).Result;
                excit.Configuration.Add(configuration);
                //SplitString(configuration);
            }
            return excit;
        }



    }
}
