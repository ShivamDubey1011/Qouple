using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //modles names paresent in after migration batabase and table will going to create
        public DbSet<Values> Values { get; set;  }
    }
}
