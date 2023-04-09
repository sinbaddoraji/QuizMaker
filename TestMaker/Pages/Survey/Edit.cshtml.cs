using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;
using TestMaker.Models;

namespace TestMaker.Pages.Survey
{
    public class EditModel : PageModel
    {
        private readonly TestMaker.Data.ApplicationDbContext _context;

        public EditModel(TestMaker.Data.ApplicationDbContext context)
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

            var survey =  await _context.Survey.FirstOrDefaultAsync(m => m.SurveyId == id);
            if (survey == null)
            {
                return NotFound();
            }

            _context.Attach(survey);
			Survey = survey;

            

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string action)
        {
	        if (!string.IsNullOrEmpty(action))
	        {
		       HandleAction(action);
                return Page();
	        }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Survey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(Survey.SurveyId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private void HandleAction(string action)
        {
	        if (string.IsNullOrEmpty(Survey.UserId))
	        {
                Survey.UserId = User.Identity.Name;
	        }

	        if (action.Equals("Add Question"))
	        {
		        AddQuestion();
			}
        }

        private void AddQuestion()
        {
			//Survey.Questions ??= new List<Question>();
			Survey.Questions.Add(new Question()
			{
				Content = "Random Question?",
                Choices =
                {
					new QuestionChoice() {Content = "Choice 1"},
					new QuestionChoice() {Content = "Choice 2"},
					new QuestionChoice() {Content = "Choice 3"},
					new QuestionChoice() {Content = "Choice 4"},
				},
                CorrectAnswerIndex = "1"
			});
		}

        private bool SurveyExists(Guid id)
        {
          return (_context.Survey?.Any(e => e.SurveyId == id)).GetValueOrDefault();
        }
    }
}
