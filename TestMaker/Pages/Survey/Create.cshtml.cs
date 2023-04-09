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

        public IActionResult AddQuestion()
        {
	        Survey.Questions ??= new List<Question>();
	        Survey.Questions.Add(new Question());

	        return Page();
        }
        

		[BindProperty]
        public Models.Survey Survey { get; set; } = new Models.Survey();
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string? username)
        {
            Survey.UserId = User.Identity.Name;
            _context.Survey.Add(Survey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
