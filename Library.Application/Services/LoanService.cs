using Library.Common.Dto;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Library.Application.Services
{
    public class PrestamoService : ILoanService
    {
        private readonly LibraryDbContext _context;

        public PrestamoService(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Actualiza la fecha de devolución de un préstamo.
        /// </summary>
        public async Task<ActualizarDevolucionDto> UpdateReturnDateAsync(int id, DateTime fechaDevolucion)
        {
            try
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
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al actualizar la fecha de devolución en la base de datos.", ex);
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Crea un nuevo préstamo asociado a un libro.
        /// To Do: Implementar PostLoan
        /// </summary>
        public async Task<bool> PostLoan(int id)
        {
            try
            {
                var libro = await _context.Books.FindAsync(id);
                if (libro == null) return false;

                var prestamo = new Loan
                {
                    Libro_id = id,
                    Fecha_prestamo = DateTime.UtcNow,
                    Fecha_devolucion = null
                };

                _context.Loans.Add(prestamo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("Error al crear el préstamo en la base de datos.", ex);
            } 
        }

        /// <summary>
        /// Elimina un préstamo existente.
        /// </summary>
        public async Task<bool> DeleteLoanAsync(int id)
        {
            var prestamo = await _context.Loans.FindAsync(id);
            if (prestamo == null)
                throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}.");

            try
            {
                _context.Loans.Remove(prestamo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al eliminar el préstamo de la base de datos.", ex);
            }
        }

        /// <summary>
        /// Obtiene los prestamos sin devolver.
        /// </summary>
        public async Task<IEnumerable<PrestamoNoDevueltoDto>> ObtenerPrestamosNoDevueltosAsync()
        {
            try
            {
                return await _context.Loans
                    .Include(p => p.Book)
                    .ThenInclude(l => l!.Autor)
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
            catch (DbException ex)
            {
                throw new InvalidOperationException("Error al obtener los préstamos no devueltos de la base de datos.", ex);
            }
        }
    }
}
