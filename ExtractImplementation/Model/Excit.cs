namespace ExtractImplementation.Model
{
    public class Excit
    {
        public int Version { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LoginInstruction { get; set; }
        public int AsiNumber { get; set; }
        public object Inventory { get; set; }
        public bool HasInventory { get; set; }
        public bool HasLogin { get; set; }
        public int? OrderStatus { get; set; }
        public bool HasOrderStatus { get; set; }
        public object OrderCreation { get; set; }
        public bool HasOrderCreation { get; set; }
        public object Pricing { get; set; }
        public bool HasPricing { get; set; }
        public object ServiceProviderLogin { get; set; }
        public bool HasServiceProviderLogin { get; set; }
        public object ProductIntegration { get; set; }
        public bool HasProductIntegration { get; set; }
    }
}
