using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;

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
                        View((await _context.TestResults.ToListAsync()).Where(x => x.UserId ==  User.Identity?.Name)) :
                        Problem("Entity set 'ApplicationDbContext.TestResults'  is null.");
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
        
    }
}
