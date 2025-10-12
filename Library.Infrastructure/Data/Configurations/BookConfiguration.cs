using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Libros");

            builder.HasKey(b => b.Libro_id);

            builder.Property(b => b.Libro_id).HasColumnName("Libro_id");
            builder.Property(b => b.Titulo).HasColumnName("Titulo").IsRequired().HasMaxLength(300);
            builder.Property(b => b.Autor_id).HasColumnName("Autor_id");
            builder.Property(b => b.Año_publicacion).HasColumnName("Año_publicacion").IsRequired();
            builder.Property(b => b.Genero).HasColumnName("Genero").HasMaxLength(100);

            // Índice en AutorId para mejorar performance en JOIN
            builder.HasIndex(b => b.Autor_id).HasDatabaseName("IX_Libros_AutorId");

            builder.HasOne(b => b.Autor)
                    .WithMany(b => b.Books)
                    .HasForeignKey(b => b.Autor_id)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
