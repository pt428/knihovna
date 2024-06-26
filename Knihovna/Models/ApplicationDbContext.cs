using Microsoft.EntityFrameworkCore;

namespace Knihovna.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Reader> Readers { get; set; }
	}
}
