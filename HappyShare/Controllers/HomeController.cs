using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HappyShare.Models;
using HappyShare.Models.SharedItemViewModels;
using HappyShare.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pvm = new SharedItemViewModel();
            pvm.Categories = await _context.Categories
                 .AsNoTracking()
                .ToListAsync();

            pvm.SharedItems = await _context.SharedItems.Include(p => p.Category).ToListAsync();

            return View(pvm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Donate()
        {
            ViewData["Message"] = "Thank you for your charity!";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
