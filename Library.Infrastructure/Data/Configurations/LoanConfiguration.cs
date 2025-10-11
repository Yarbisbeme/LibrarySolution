using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Prestamos");

            builder.HasKey(l => l.Prestamo_id);

            builder.Property(l => l.Prestamo_id).HasColumnName("Prestamo_id");
            builder.Property(l => l.Libro_id).HasColumnName("Libro_id");
            builder.Property(l => l.Fecha_prestamo).HasColumnName("Fecha_prestamo").IsRequired();
            builder.Property(l => l.Fecha_devolucion).HasColumnName("Fecha_devolucion");

            // Indices
            builder.HasIndex(l => l.Libro_id).HasDatabaseName("IX_Prestamos_libro_id");
            builder.HasIndex(l => l.Fecha_devolucion).HasDatabaseName("IX_Prestamos_FechaDevolucion");

            // Relaciones
            builder.HasOne(l => l.Book)
                    .WithMany(b => b.Loans)
                    .HasForeignKey(l => l.Libro_id)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
