using Microsoft.AspNetCore.Mvc;
using EAPD7111_Part2.Models;
using Microsoft.EntityFrameworkCore;

namespace EAPD7111_Part2.Controllers
{
    public class ClientC : Controller
    {
        private readonly GlmsDbContext _context;

        public ClientC(GlmsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .Include(c => c.Contracts)
                .ThenInclude(contract => contract.ServiceRequests)
                .ToListAsync();

            return View("~/Views/Clients/Index.cshtml", clients);
        }

        public IActionResult Create() => View("~/Views/Clients/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // ✅ Reload Index with updated data
                var clients = await _context.Clients
                    .Include(c => c.Contracts)
                    .ThenInclude(contract => contract.ServiceRequests)
                    .ToListAsync();

                return View("~/Views/Clients/Index.cshtml", clients);
            }

            return View("~/Views/Clients/Create.cshtml", client);
        }
    }
}
