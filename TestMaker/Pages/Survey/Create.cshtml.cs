using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestMaker.Data;
using TestMaker.Models;

namespace TestMaker.Pages.Survey
{
    public class CreateModel : PageModel
    {
        private readonly TestMaker.Data.ApplicationDbContext _context;

        public CreateModel(TestMaker.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Survey Survey { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Survey == null || Survey == null)
            {
                return Page();
            }

            _context.Survey.Add(Survey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
