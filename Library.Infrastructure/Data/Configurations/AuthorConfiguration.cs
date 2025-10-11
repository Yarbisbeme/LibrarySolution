using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Autores");

            builder.HasKey(a => a.Autor_id);

            builder.Property(a => a.Autor_id).HasColumnName("Autor_id");
            builder.Property(a => a.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(100);
            builder.Property(a => a.Nacionalidad).HasColumnName("Nacionalidad").IsRequired().HasMaxLength(100);

            builder.HasMany(a => a.Books)
                    .WithOne(b => b.Autor)
                    .HasForeignKey(b => b.Autor_id)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
