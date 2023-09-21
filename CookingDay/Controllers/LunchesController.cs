using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookingDay.Data;
using CookingDay.Models;
using Microsoft.AspNetCore.Authorization;

namespace CookingDay.Controllers
{
    public class LunchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LunchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lunches
        
        public async Task<IActionResult> Index()
        {
              return _context.Lunch != null ? 
                          View(await _context.Lunch.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Lunch'  is null.");
        }

        //ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        public async Task<IActionResult> ShowSearchResult(string SearchPhrase)
        {
            return View("Index", await _context.Lunch.Where(c => c.MealName.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Lunches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lunch == null)
            {
                return NotFound();
            }

            var lunch = await _context.Lunch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lunch == null)
            {
                return NotFound();
            }

            return View(lunch);
        }

        // GET: Lunches/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lunches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MealName,MealRecipe")] Lunch lunch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lunch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lunch);
        }

        // GET: Lunches/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lunch == null)
            {
                return NotFound();
            }

            var lunch = await _context.Lunch.FindAsync(id);
            if (lunch == null)
            {
                return NotFound();
            }
            return View(lunch);
        }

        // POST: Lunches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MealName,MealRecipe")] Lunch lunch)
        {
            if (id != lunch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lunch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LunchExists(lunch.Id))
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
            return View(lunch);
        }

        // GET: Lunches/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lunch == null)
            {
                return NotFound();
            }

            var lunch = await _context.Lunch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lunch == null)
            {
                return NotFound();
            }

            return View(lunch);
        }

        // POST: Lunches/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lunch == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lunch'  is null.");
            }
            var lunch = await _context.Lunch.FindAsync(id);
            if (lunch != null)
            {
                _context.Lunch.Remove(lunch);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LunchExists(int id)
        {
          return (_context.Lunch?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
