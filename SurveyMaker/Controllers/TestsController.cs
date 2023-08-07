using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMaker.Data;
using TestMaker.Helpers.Implementation;
using TestMaker.Models;
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
        public async Task<IActionResult> Create([Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] Test test)
        {
            if (!ModelState.IsValid) 
                return View(test);

            test.TestId = Guid.NewGuid();
            test.UserId = User.Identity.Name;
            _context.Add(test);
            await _context.SaveChangesAsync();

            var testWrapper = new TestWrapper()
            {
                CreatedDate = test.CreatedDate,
                Description = test.Description,
                Name = test.Name,
                TestId = test.TestId,
                UserId = test.UserId,
                Questions = new SafeJsonSerializer().Deserialize<List<Question>>(test.Questions)
            };
            return View("Edit", testWrapper);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper testWrapper)
        {
            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.TestId == testWrapper.TestId);

            //Make sure that the questions have the same number of answer states as answers
            testWrapper.Questions ??= new List<Question>();

            for (int i = 0; i < testWrapper.Questions.Count; i++)
            {
                if (testWrapper.Questions[i].AnswersState == null)
                {
                    testWrapper.Questions[i].AnswersState = Enumerable.Repeat(false, testWrapper.Questions[i].Answers.Count).ToList();
                }
                else if (testWrapper.Questions[i].AnswersState.Count < testWrapper.Questions[i].Answers.Count)
                {
                    testWrapper.Questions[i].AnswersState.AddRange(Enumerable.Repeat(false, testWrapper.Questions[i].Answers.Count - testWrapper.Questions[i].AnswersState.Count));
                }
            }

            var serializer = new SafeJsonSerializer();

            test.Name = testWrapper.Name;
            test.Description = testWrapper.Description;
            test.Questions = serializer.Serialize(testWrapper.Questions);
            
            try
            {
                _context.Update(test);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public Task<IActionResult> AddQuestion(Guid testId, [Bind("TestId,Name,Description,UserId,CreatedDate,Questions")] TestWrapper Test)
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

            return Task.FromResult<IActionResult>(View("Edit", Test));
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
