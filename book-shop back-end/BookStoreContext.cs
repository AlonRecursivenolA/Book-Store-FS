using book_shop_back_end.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace book_shop_back_end.Data
{
    public class BookStoreContext : IdentityDbContext<User>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);



        //    modelBuilder.Entity<Book>()
        //        .Property(b => b.Title)
        //        .IsRequired()
        //        .HasMaxLength(200);
        //}
    }
}
