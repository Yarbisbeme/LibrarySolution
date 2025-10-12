using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Library.Infrastructure.Data.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Prestamos");
            builder.HasKey(e => e.Prestamo_id);
            builder.Property(e => e.Prestamo_id).HasColumnName("prestamo_id");
            builder.Property(e => e.Libro_id).HasColumnName("libro_id");
            builder.Property(e => e.Fecha_prestamo).HasColumnName("fecha_prestamo").IsRequired();
            builder.Property(e => e.Fecha_devolucion).HasColumnName("fecha_devolucion");

            // Índice en LibroId para mejorar performance en JOIN
            builder.HasIndex(e => e.Libro_id).HasDatabaseName("IX_Prestamos_LibroId");

            // Índice en FechaDevolucion para optimizar consulta de préstamos no devueltos
            // Esta es la optimización clave para GET /prestamos/no-devueltos
            builder.HasIndex(e => e.Fecha_devolucion).HasDatabaseName("IX_Prestamos_FechaDevolucion");

            builder.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.Libro_id)
                .HasConstraintName("FK_Prestamos_Libros_Libro_id")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
