using DatingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class DataContext : DbContext
    {
        //this class will be used as service in our application
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        //Dbset represent tables inside our database. So if we give this property Users, a table of name Users will get created.
        public DbSet<AppUser> Users { get; set; }
    }
}