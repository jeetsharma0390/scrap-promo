using System;
using System.Collections.Generic;
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
                Console.WriteLine($"ExtractImplementation - Error occured while getting list of supplier {ex.Message.ToString()}");
                throw ex;
            }
            foreach (var supplier in supplierDetails)
            {
                var configuration = excitClient.GetSupplierConfiguration(supplier.CompanyId).Result;
                excit.Configuration.Add(configuration);
            }
            return excit;
        }



    }
}
