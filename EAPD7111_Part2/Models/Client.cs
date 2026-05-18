using Microsoft.AspNetCore.Mvc;

namespace EAPD7111_Part2.Models
{
    public class Client

    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string Region { get; set; }

        // Navigation property
        public ICollection<Contract> Contracts { get; set; }
    }
}
