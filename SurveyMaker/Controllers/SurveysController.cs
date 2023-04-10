using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyMaker.Data;
using TestMaker.Models;

namespace SurveyMaker.Controllers
{
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Surveys
        public async Task<IActionResult> Index()
        {
            return _context.Survey != null ?
                        View(await _context.Survey.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Survey'  is null.");
        }

        // GET: Surveys/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Survey == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey
                .FirstOrDefaultAsync(m => m.SurveyId == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // GET: Surveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SurveyId,Name,Description,UserId,CreatedDate,Timestamp")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                survey.SurveyId = Guid.NewGuid();
                survey.UserId = User.Identity.Name;

                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Survey == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("SurveyId,Name,Description,UserId,CreatedDate,Timestamp")] Survey survey)
        {

	        if (id == null)
	        {
		        survey.Questions.Add(new Question()
		        {
			        Choices = new List<QuestionChoice>()
			        {
				        new QuestionChoice() { Content = "Choice 1" },
				        new QuestionChoice() { Content = "Choice 2" },
				        new QuestionChoice() { Content = "Choice 3" },
				        new QuestionChoice() { Content = "Choice 4" }
			        },
			        Content = "Question?",
		        });

		        if (ModelState.IsValid)
		        {
			        _context.Update(survey);
			        //await _context.SaveChangesAsync();
			        //return RedirectToAction(nameof(Index));
		        }

		        return View(survey);
			}

			if (id != survey.SurveyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.SurveyId))
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
            return View(survey);
        }

        


		// GET: Surveys/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Survey == null)
            {
                return NotFound();
            }

            var survey = await _context.Survey
                .FirstOrDefaultAsync(m => m.SurveyId == id);
            if (survey == null)
            {
                return NotFound();
            }

			return RedirectToAction(nameof(Index));
		}

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Survey == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Survey'  is null.");
            }
            var survey = await _context.Survey.FindAsync(id);
            if (survey != null)
            {
                _context.Survey.Remove(survey);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyExists(Guid id)
        {
            return (_context.Survey?.Any(e => e.SurveyId == id)).GetValueOrDefault();
        }
    }
}
