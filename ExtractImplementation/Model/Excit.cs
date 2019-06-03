namespace ExtractImplementation.Model
{
    public class Excit
    {
        public int Version { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int AsiNumber { get; set; }
        public object Inventory { get; set; }
        public bool HasInventory { get; set; }
        public bool HasLogin { get; set; }
        public bool HasOrderStatus { get; set; }
        public bool HasOrderCreation { get; set; }
        public bool HasPricing { get; set; }
        public bool HasServiceProviderLogin { get; set; }
        public bool HasProductIntegration { get; set; }
    }
}
