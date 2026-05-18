using System;
using System.Threading.Tasks;
using EAPD7111_Part2.Models;
using Microsoft.EntityFrameworkCore;

namespace EAPD7111_Part2.Service
{
    // ✅ Plain service class, not a Controller
    public class ServiceRequestS
    {
        private readonly GlmsDbContext _context;

        public ServiceRequestS(GlmsDbContext context)
        {
            _context = context;
        }

        // ✅ Create a new service request with contract validation
        public async Task<bool> CreateServiceRequest(ServiceRequest request)
        {
            // Check contract exists
            var contract = await _context.Contracts.FindAsync(request.ContractId);

            if (contract == null)
                throw new InvalidOperationException("Contract not found.");

            // Prevent requests for inactive contracts
            if (contract.Status == "Expired" || contract.Status == "OnHold")
                throw new InvalidOperationException("Cannot create request for inactive contract.");

            // Save request
            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Validate file uploads (only PDFs allowed)
        public Task<bool> ValidateFileUpload(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) ||
                !fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only PDF files are allowed.");
            }

            return Task.FromResult(true);
        }

        // (Optional) Retrieve all service requests
        public async Task<List<ServiceRequest>> GetAllServiceRequests()
        {
            return await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();
        }
    }
}
