using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ThucHanhAPI2.Model.Domain;

namespace ThucHanhAPI2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book_Author>()
            .HasOne(b => b.Book)
            .WithMany(ba => ba.Book_Author)
            .HasForeignKey(bi => bi.BookId);
            modelBuilder.Entity<Book_Author>()
            .HasOne(b => b.Author)
            .WithMany(ba => ba.Book_Authors)
            .HasForeignKey(bi => bi.Id);
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Book_Author> Books_Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
    }
}
