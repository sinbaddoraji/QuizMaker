using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;
using TestMaker.Data.Migrations;
using TestMaker.Helpers.Implementation;
using TestMaker.Models;
using static System.Net.Mime.MediaTypeNames;
using Test = TestMaker.Models.Test;
using TestResults = TestMaker.Models.TestResults;

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

                var testWrapper = new TestWrapper()
                {
                    CreatedDate = Test.CreatedDate,
                    Description = Test.Description,
                    Name = Test.Name,
                    TestId = Test.TestId,
                    UserId = Test.UserId,
                    Questions = new SafeJsonSerializer().Deserialize<List<Question>>(Test.Questions)
                };
                return View("Edit", testWrapper);
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

            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            var testWrapper = new TestWrapper()
            {
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Name = test.Name,
                TestId = test.TestId,
                UserId = test.UserId,
                Questions = new SafeJsonSerializer().Deserialize<List<Question>>(test.Questions)
            };
            return View(testWrapper);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
        {
            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == Test.TestId);

            //Make sure that the questions have the same number of answer states as answers
            if (Test.Questions == null)
            {
                Test.Questions = new List<Question>();
            }

            for (int i = 0; i < Test.Questions.Count; i++)
            {
                if (Test.Questions[i].AnswersState == null)
                {
                    Test.Questions[i].AnswersState = Enumerable.Repeat(false, Test.Questions[i].Answers.Count).ToList();
                }
                else if (Test.Questions[i].AnswersState.Count < Test.Questions[i].Answers.Count)
                {
                    Test.Questions[i].AnswersState.AddRange(Enumerable.Repeat(false, Test.Questions[i].Answers.Count - Test.Questions[i].AnswersState.Count));
                }
            }

            var serializer = new SafeJsonSerializer();

            test.Name = Test.Name;
            test.Description = Test.Description;
            test.Questions = serializer.Serialize(Test.Questions);

            

            if (ModelState.IsValid)
            {
                try
                {
                    // tRY MANUALLY GETTING THE QUESTIONS FROM THE FORM

                    _context.Update(test);
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

        [HttpPost]
        public async Task<IActionResult> AddQuestion(Guid testId, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
        {
            if (Test.Questions == null)
            {
                Test.Questions = new List<Question>();
            }

            Test.Questions.Add(new Question()
            {
                Answers = new List<string>()
                {
                    "Option1", "Option2", "Option3"
                },
                AnswersState = new List<bool>()
                {
                    false, false, false
                },
                QuestionContent = "Question Name",
                QuestionId = Guid.NewGuid()
            });

            return View("Edit", Test);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOption(Guid testId, int questionIndex, int optionIndex, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    // Append the new question to the existing Questions property

                    Test.Questions[questionIndex].Answers.RemoveAt(optionIndex);
                    Test.Questions[questionIndex].AnswersState.RemoveAt(optionIndex);

                    return View("Edit", Test);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle the exception if necessary
                }
            }

            return View("Edit", Test);
        }

        [HttpPost]
        public async Task<IActionResult> AddOption(Guid testId, int questionIndex, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Test.Questions[questionIndex].Answers.Add("New Option");
                    Test.Questions[questionIndex].AnswersState.Add(false);

                    //await _context.SaveChangesAsync();

                    return View("Edit", Test);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle the exception if necessary
                }
            }

            return View("Edit", Test);
        }
        

        [HttpPost]
        public async Task<IActionResult> RemoveQuestion(Guid testId, int questionIndex, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
        {
            try
            {
                Test.Questions.RemoveAt(questionIndex);
                
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return View("Edit", Test);
        }

        public async Task SaveTestWrapper(TestWrapper testWrapper)
        {
            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == testWrapper.TestId);

            var serializer = new SafeJsonSerializer();
            test.Questions = serializer.Serialize(testWrapper.Questions);

            _context.Update(test);
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
            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            var testWrapper = new TestWrapper()
            {
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Name = test.Name,
                TestId = test.TestId,
                UserId = test.UserId,
                Questions = new SafeJsonSerializer().Deserialize<List<Question>>(test.Questions)
            };

            // Clear answer state of all questions 
            for (var index = 0; index < testWrapper.Questions.Count; index++)
            {
                for (int j = 0; j < testWrapper.Questions[index].AnswersState.Count; j++)
                {
                    testWrapper.Questions[index].AnswersState[j] = false;
                }
            }

            return View(testWrapper);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitScores(TestWrapper testTaken)
        {
            int totalAnswers = 0;
            int totalCorrectAnswers = 0;

            var test = await _context.Test.FindAsync(testTaken.TestId);

            var originalTest = new TestWrapper()
            {
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Name = test.Name,
                TestId = test.TestId,
                UserId = test.UserId,
                Questions = new SafeJsonSerializer().Deserialize<List<Question>>(test.Questions)
            };


            // Compare qustions `AnswersState` with original test `AnswersState`
            for (var index = 0; index < testTaken.Questions.Count; index++)
            {
                for (int j = 0; j < testTaken.Questions[index].AnswersState.Count; j++)
                {
                    if (originalTest.Questions[index].AnswersState[j])
                    {
                        totalAnswers++;
                        if (testTaken.Questions[index].AnswersState[j] == originalTest.Questions[index].AnswersState[j])
                        {
                            totalCorrectAnswers++;
                        }
                    }
                    
                }
            }
            
            int score = (int)((double)((double)totalCorrectAnswers / (double)totalAnswers) * 100.00);
            var newTestResults = new TestResults
            {
                UserId = User.Identity?.Name,
                TestId = testTaken.TestId,
                TestName = testTaken.Name,
                TestDescription = testTaken.Description,
                Score = score
            };

            _context.TestResults?.Add(newTestResults);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), "TestResults");
        }

    }
}
