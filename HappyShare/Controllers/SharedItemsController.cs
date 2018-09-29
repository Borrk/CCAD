using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HappyShare.Data;
using HappyShare.Models;
using HappyShare.Models.SharedItemViewModels;

namespace HappyShare.Controllers
{
    public class SharedItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SharedItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SharedItems
        public async Task<IActionResult> Index(string searchString, int? categoryID)
        {
            ViewData["CurrentFilter"] = searchString;

            var pvm = new SharedItemViewModel
            {
                Categories = await _context.Categories.Include(c => c.SharedItems).AsNoTracking().ToListAsync()               
            };

            // All the categories
            if (categoryID == null || categoryID <= 0)
                pvm.SharedItems = await _context.SharedItems.Include(p => p.Category).ToListAsync();
            else
                pvm.SharedItems = await _context.SharedItems.Where(c => c.Category.CategoryID == categoryID)
                    .Include(p => p.Category).ToListAsync();

            // searching condition
            if (!string.IsNullOrEmpty(searchString))
            {
                pvm.SharedItems = pvm.SharedItems.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return View(pvm);
        }

        // GET: SharedItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharedItem = await _context.SharedItems
                .SingleOrDefaultAsync(m => m.ID == id);
            if (sharedItem == null)
            {
                return NotFound();
            }

            return View(sharedItem);
        }

        // GET: SharedItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SharedItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,PictureLink,Location,Address,ContactorPhone,ContactorEmail,Type,Description")] SharedItem sharedItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sharedItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sharedItem);
        }

        // GET: SharedItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharedItem = await _context.SharedItems.SingleOrDefaultAsync(m => m.ID == id);
            if (sharedItem == null)
            {
                return NotFound();
            }
            return View(sharedItem);
        }

        // POST: SharedItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,PictureLink,Location,Address,ContactorPhone,ContactorEmail,Type,Description")] SharedItem sharedItem)
        {
            if (id != sharedItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sharedItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SharedItemExists(sharedItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sharedItem);
        }

        // GET: SharedItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharedItem = await _context.SharedItems
                .SingleOrDefaultAsync(m => m.ID == id);
            if (sharedItem == null)
            {
                return NotFound();
            }

            return View(sharedItem);
        }

        // POST: SharedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sharedItem = await _context.SharedItems.SingleOrDefaultAsync(m => m.ID == id);
            _context.SharedItems.Remove(sharedItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SharedItemExists(int id)
        {
            return _context.SharedItems.Any(e => e.ID == id);
        }
    }
}
