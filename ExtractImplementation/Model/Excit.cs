using ASI.Contracts.Excit.Supplier.Version1;
using System.Collections.Generic;

namespace ExtractImplementation.Model
{
    public class Excit
    {
        public List<SupplierAPI> SupplierAPIs { get; set; }
        public List<Configuration> Configuration { get; set; }
    }
}
