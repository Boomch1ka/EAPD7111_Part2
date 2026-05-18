using Microsoft.AspNetCore.Mvc;

namespace EAPD7111_Part2.Models
{
    public class ServiceRequest 
    {
        public int ServiceRequestId { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        public string Description { get; set; }
        public decimal CostUSD { get; set; }
        public decimal CostZAR { get; set; }
        public string Status { get; set; }
    }
}
