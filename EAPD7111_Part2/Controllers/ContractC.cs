using Microsoft.AspNetCore.Mvc;
using EAPD7111_Part2.Models;
using Microsoft.EntityFrameworkCore;

namespace EAPD7111_Part2.Controllers
{
    public class ContractC : Controller
    {
        private readonly GlmsDbContext _context;

        public ContractC(GlmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .ToListAsync();

            return View("~/Views/Contracts/Index.cshtml", contracts);
        }

        public IActionResult Create() => View("~/Views/Contracts/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile signedAgreement)
        {
            if (ModelState.IsValid)
            {
                // ✅ Handle PDF upload
                if (signedAgreement != null && Path.GetExtension(signedAgreement.FileName).ToLower() == ".pdf")
                {
                    var folderPath = Path.Combine("wwwroot", "agreements");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = $"{Guid.NewGuid()}.pdf";
                    var filePath = Path.Combine(folderPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await signedAgreement.CopyToAsync(stream);

                    contract.SignedAgreementPath = $"/agreements/{fileName}";
                }

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();

                // ✅ Reload Index with updated data
                var contracts = await _context.Contracts
                    .Include(c => c.Client)
                    .Include(c => c.ServiceRequests)
                    .ToListAsync();

                return View("~/Views/Contracts/Index.cshtml", contracts);
            }

            return View("~/Views/Contracts/Create.cshtml", contract);
        }
    }
}
