using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services
{
    public class LibroService : IBookService
    {
        private readonly LibraryDbContext _context;

        public LibroService(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<LibroResponseDto> CrearLibroAsync(LibroCreateDto dto)
        {
            var autor = await _context.Authors.FindAsync(dto.AutorId);
            if (autor == null)
                throw new KeyNotFoundException($"No se encontró el autor con el ID {dto.AutorId} proporcionado.");
            try
            {
                var libro = new Book
                {
                    Titulo = dto.Titulo,
                    Año_publicacion = dto.AnioPublicacion,
                    Autor_id = dto.AutorId,
                    Genero = dto.Genero
                };
                _context.Books.Add(libro);
                await _context.SaveChangesAsync();

                return new LibroResponseDto
                {
                    LibroId = libro.Libro_id,
                    Titulo = libro.Titulo,
                    AnioPublicacion = libro.Año_publicacion,
                    Autor = autor.Nombre,
                    Genero = libro.Genero
                };

            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al guardar el libro en la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<LibroResponseDto>> ObtenerLibrosAntesDe2000Async()
        {
            try
            {
                return await _context.Books
                    .Where(l => l.Año_publicacion < 2000)
                    .Select(l => new LibroResponseDto
                    {
                        LibroId = l.Libro_id,
                        Titulo = l.Titulo,
                        AnioPublicacion = l.Año_publicacion,
                        Genero = l.Genero ?? "No especificado",
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener la lista de libros.", ex);
            }
        }
    }
}
