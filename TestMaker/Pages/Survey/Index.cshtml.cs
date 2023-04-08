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
    public class IndexModel : PageModel
    {
        private readonly TestMaker.Data.ApplicationDbContext _context;

        public IndexModel(TestMaker.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Survey> Survey { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Survey != null)
            {
                Survey = await _context.Survey.ToListAsync();
            }
        }
    }
}
