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
using Newtonsoft.Json.Linq;

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
        public async  Task<IActionResult> ManageItems()
        {
            var items = await _context.SharedItems.Include(p => p.Category).AsNoTracking().ToListAsync();
            return View(items);
        }

        // GET: SharedItems
        public async Task<IActionResult> Index(string searchString, int? categoryID)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["categoryID"] = categoryID ?? 0;

            var pvm = new SharedItemViewModel
            {
                Categories = await _context.Categories.Include(c => c.SharedItems).AsNoTracking().ToListAsync()               
            };

            // All the categories
            if (categoryID == null || categoryID <= 0)
                pvm.SharedItems = await _context.SharedItems.Include(p => p.Category).OrderByDescending( p => p.PostTime).ToListAsync();
            else
                pvm.SharedItems = await _context.SharedItems.Where(c => c.Category.CategoryID == categoryID)
                    .Include(p => p.Category).OrderByDescending(p => p.PostTime).ToListAsync();

            // searching condition
            if (!string.IsNullOrEmpty(searchString))
            {
                pvm.SharedItems = pvm.SharedItems.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).OrderByDescending(p => p.PostTime).ToList();
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

            var sharedItem = await _context.SharedItems.Include( i=>i.Category)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Address,ContactorPhone,ContactorEmail,Type,Description")] SharedItem sharedItem)
        {
            if (id != sharedItem.ID)
            {
                return NotFound();
            }

            var oldItem = _context.SharedItems.Single(i => i.ID == id);
            if ( oldItem == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oldItem.Name = sharedItem.Name;
                    oldItem.Type = sharedItem.Type;
                    oldItem.Description = sharedItem.Description;
                    oldItem.ContactorEmail = sharedItem.ContactorEmail;
                    oldItem.ContactorPhone = sharedItem.ContactorPhone;
                    oldItem.Address = sharedItem.Address;

                    _context.Update(oldItem);
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
                return RedirectToAction(nameof(ManageItems));
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
            return RedirectToAction(nameof(ManageItems));
        }

        private bool SharedItemExists(int id)
        {
            return _context.SharedItems.Any(e => e.ID == id);
        }
    }
}
