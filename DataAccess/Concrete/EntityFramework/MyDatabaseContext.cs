using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class MyDatabaseContext:DbContext
    {
        public MyDatabaseContext()
        {
        }
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options):base(options)
        {
        }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        public DbSet<Navbar> Navbars { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsDetail>NewsDetail { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Footer> Footer { get; set; }
        public DbSet<FormsActive> FormsActive { get; set; }

    }
}
