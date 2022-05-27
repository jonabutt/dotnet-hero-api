using HeroAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HeroAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {

        }

        public DbSet<Hero> Heroes { get; set; }
    }
}
