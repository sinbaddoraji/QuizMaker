using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestMaker.Models;

namespace TestMaker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
		public DbSet<TestMaker.Models.Test>? Test { get; set; }

        public DbSet<TestMaker.Models.TestResults>? TestResults { get; set; }
	}
}