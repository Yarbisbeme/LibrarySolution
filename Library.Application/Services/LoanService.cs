using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Library.Common.Dto;

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
        public async Task<LoanResponse> PostLoan(PostPrestamoDto Dto)
        {
            var libro = await _context.Books
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(l => l.Libro_id == Dto.BookId);
            if (libro == null) throw new KeyNotFoundException($"No se encontró el libro con ID {Dto.BookId}");
            
            try
            {

                int id = Dto.BookId;
                var prestamo = new Loan
                {
                    Libro_id = id,
                    Fecha_prestamo = DateTime.UtcNow,
                    Fecha_devolucion = Dto.Devolucion_Prestamo
                };

                _context.Loans.Add(prestamo);
                await _context.SaveChangesAsync();

                return new LoanResponse { 
                    LibroId = prestamo.Libro_id,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor.Nombre,
                    Fecha_Prestamo = prestamo.Fecha_prestamo,
                    Fecha_Devolucion = prestamo.Fecha_devolucion
                };
            }
            catch (DbUpdateException ex)
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
                    .Where(p => p.Fecha_devolucion > DateTime.UtcNow || p.Fecha_devolucion == null)
                    .Select(p => new PrestamoNoDevueltoDto
                    {
                        PrestamoId = p.Prestamo_id,
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
