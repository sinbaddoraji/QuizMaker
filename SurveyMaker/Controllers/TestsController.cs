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
                Test.UserId = User.Identity.Name;
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
                    Test.UserId = User.Identity.Name;
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

        public async Task<IActionResult> Share(Guid id)
        {
	        var Test = await _context.Test
		        .FirstOrDefaultAsync(m => m.TestId == id);

	        List<QuestionModel> questionList = new List<QuestionModel>();
	        foreach (var question in Test.Questions.Split("\n"))
	        {
				//\"Question Name\" - *\"Option 1\" - \"Option 2\" - \"Option 3\"
				var q = question.Split("-");
				QuestionModel questionObject = new QuestionModel();

				questionObject.Content = q[0].Replace("\"", "").Trim(' ');

				for (int i = 1; i < q.Length; i++)
				{
					var current = q[i].Replace("\"", "").Trim(' ');
                    
					current = current.Trim('*', ' ');
					if (q[i].Trim(' ').StartsWith("*"))
                    {
                        questionObject.CorrectAnswerIndex = i - 1;
                    }

					questionObject.Choices.Add(current);
				}
				questionList.Add(questionObject);
			}

	        TestModel t = new TestModel()
	        {
		        Tests = questionList,
		        id = Test.TestId,
                TestName = Test.Name,
                TestDescription = Test.Description
	        };

			return View(t);
        }

        [HttpPost]
        public ActionResult SubmitScores(TestModel model)
        {
            int totalQuestions = model.Tests.Count;
            int totalCorrectAnswers = 0;

            foreach (var question in model.Tests)
            {
                if (question.SelectedIndex == question.CorrectAnswerIndex)
                {
                    totalCorrectAnswers++;
                }
            }

            float score = (float)totalCorrectAnswers / totalQuestions * 100;

            ViewBag.Score = score;

            var newTestResults = new TestResults
            {
	            UserId = User.Identity?.Name,
	            TestId = model.id,
	            TestName = model.TestName,
	            TestDescription = model.TestDescription,
	            Score = Convert.ToInt32(score)
            };

            _context.TestResults?.Add(newTestResults);
            _context.SaveChanges();

			return RedirectToAction(nameof(Index), "TestResults");
        }

    }
}
