using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;
using TestMaker.Models;

namespace TestMaker.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            return _context.Test != null ?
                        View(await _context.Test.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Test'  is null.");
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var Test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (Test == null)
            {
                return NotFound();
            }

            return View(Test);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            return View(new Test());
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] Test Test)
        {
            if (ModelState.IsValid)
            {
                Test.TestId = Guid.NewGuid();
                _context.Add(Test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Test);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var Test = await _context.Test.FindAsync(id);
            if (Test == null)
            {
                return NotFound();
            }
            return View(Test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] Test Test)
        {
            if (id != Test.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(Test.TestId))
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
            return View(Test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var Test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (Test == null)
            {
                return NotFound();
            }

            return View(Test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Test == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Test'  is null.");
            }
            var Test = await _context.Test.FindAsync(id);
            if (Test != null)
            {
                _context.Test.Remove(Test);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(Guid id)
        {
            return (_context.Test?.Any(e => e.TestId == id)).GetValueOrDefault();
        }
    }
}
