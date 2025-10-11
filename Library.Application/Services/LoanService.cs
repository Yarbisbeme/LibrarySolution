using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services
{
    public class PrestamoService : ILoanService
    {
        private readonly LibraryDbContext _context;

        public PrestamoService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ActualizarDevolucionDto> UpdateReturnDateAsync(int id, DateTime fechaDevolucion)
        {
            var prestamo = await _context.Loans.FindAsync(id);
            if (prestamo == null)
                throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

            prestamo.Fecha_devolucion = fechaDevolucion;
            await _context.SaveChangesAsync();

            return new ActualizarDevolucionDto
            {
                FechaDevolucion = fechaDevolucion
            };
        }


        public async Task<bool> DeleteLoanAsync(int id)
        {
            var prestamo = await _context.Loans.FindAsync(id);
            if (prestamo == null) return false;

            _context.Loans.Remove(prestamo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PrestamoNoDevueltoDto>> ObtenerPrestamosNoDevueltosAsync()
        {
            return await _context.Loans
                .Include(p => p.Book)
                .ThenInclude(l => l.Autor)
                .Where(p => p.Fecha_devolucion == null)
                .Select(p => new PrestamoNoDevueltoDto
                {
                    AutorId = p.Book!.Autor!.Autor_id,
                    Nombre = p.Book.Autor.Nombre,
                    LibroId = p.Book.Libro_id,
                    Titulo = p.Book.Titulo
                })
                .ToListAsync();
        }

    }
}
