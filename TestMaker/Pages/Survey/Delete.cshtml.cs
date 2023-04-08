using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;
using TestMaker.Models;

namespace TestMaker.Pages.Survey
{
    public class DeleteModel : PageModel
    {
        private readonly TestMaker.Data.ApplicationDbContext _context;

        public DeleteModel(TestMaker.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Models.Survey Survey { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Survey == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey.FirstOrDefaultAsync(m => m.SurveyId == id);

            if (survey == null)
            {
                return NotFound();
            }
            else 
            {
                Survey = survey;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Survey == null)
            {
                return NotFound();
            }
            var survey = await _context.Survey.FindAsync(id);

            if (survey != null)
            {
                Survey = survey;
                _context.Survey.Remove(Survey);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
