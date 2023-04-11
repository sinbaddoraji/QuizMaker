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
    public class TestResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestResults
        public async Task<IActionResult> Index()
        {
            return _context.TestResults != null ?
                        View(await _context.TestResults.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.TestResults'  is null.");
        }

        // GET: TestResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TestResults == null)
            {
                return NotFound();
            }

            var testResults = await _context.TestResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testResults == null)
            {
                return NotFound();
            }

            return View(testResults);
        }
        
        

        // POST: TestResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TestResults == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TestResults'  is null.");
            }
            var testResults = await _context.TestResults.FindAsync(id);
            if (testResults != null)
            {
                _context.TestResults.Remove(testResults);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestResultsExists(int id)
        {
            return (_context.TestResults?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
