using Web.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Database
{
    public class StoreContext : DbContext
    {
        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BooksDb");
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Visualization> Visualizations { get; set; }
    }
}