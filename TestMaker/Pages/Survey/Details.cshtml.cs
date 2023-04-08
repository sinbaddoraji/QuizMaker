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
    public class DetailsModel : PageModel
    {
        private readonly TestMaker.Data.ApplicationDbContext _context;

        public DetailsModel(TestMaker.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
