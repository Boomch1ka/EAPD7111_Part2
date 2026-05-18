using Microsoft.AspNetCore.Mvc;
using EAPD7111_Part2.Models;
using EAPD7111_Part2.Service;
using Microsoft.EntityFrameworkCore;

namespace EAPD7111_Part2.Controllers
{
    public class ServiceRequestsC : Controller
    {
        private readonly ServiceRequestS _serviceRequestService;
        private readonly CurrencyS _currencyService;
        private readonly GlmsDbContext _context;

        public ServiceRequestsC(ServiceRequestS serviceRequestService, CurrencyS currencyService, GlmsDbContext context)
        {
            _serviceRequestService = serviceRequestService;
            _currencyService = currencyService;
            _context = context;
        }

        // ✅ List all service requests
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return View("~/Views/ServiceRequests/Index.cshtml", requests);
        }

        // ✅ Show create form
        public IActionResult Create(int contractId)
        {
            return View("~/Views/ServiceRequests/Create.cshtml", new ServiceRequest { ContractId = contractId });
        }

        // ✅ Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Show validation errors
                return View("~/Views/ServiceRequests/Create.cshtml", request);
            }

            try
            {
                // Convert USD → ZAR before saving
                var conversion = await _currencyService.ConvertUsdToZar(request.CostUSD);
                request.CostZAR = conversion.Value;

                if (conversion.UsedFallback)
                {
                    ModelState.AddModelError("", "⚠ Live exchange rate unavailable. Using fallback rate.");
                    return View("~/Views/ServiceRequests/Create.cshtml", request);
                }

                // ✅ Ensure the service actually saves
                await _serviceRequestService.CreateServiceRequest(request);

                // ✅ Redirect back to Index after saving
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("~/Views/ServiceRequests/Create.cshtml", request);
            }
        }
    }
}
