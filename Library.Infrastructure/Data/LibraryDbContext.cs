using Library.Domain.Common;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Loan> Loans => Set<Loan>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.updatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.createdAt = DateTime.UtcNow;
                    entry.Entity.updatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Autor)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.Autor_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Libro -> Prestamos
            modelBuilder.Entity<Loan>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(p => p.Libro_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}