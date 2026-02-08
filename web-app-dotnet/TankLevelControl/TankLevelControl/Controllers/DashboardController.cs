using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TankLevelControl.Data;

namespace TankLevelControl.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tanks = await _context.Tanks
                .OrderBy(t => t.Id)
                .ToListAsync();

            return View(tanks);
        }
    }
}
