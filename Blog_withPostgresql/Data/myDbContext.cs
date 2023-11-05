using Blog.BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_withPostgresql.Data
{
    public class myDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public myDbContext(DbContextOptions<myDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
