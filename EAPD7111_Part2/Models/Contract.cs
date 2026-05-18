namespace EAPD7111_Part2.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // Draft, Active, Expired, OnHold
        public string ServiceLevel { get; set; }

        public string SignedAgreementPath { get; set; } // PDF file path

        // Navigation property
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
