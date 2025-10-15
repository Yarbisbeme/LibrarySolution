
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

        #region ObtenerLibrosAntesDe2000Async
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
                        Autor = l.Autor.Nombre,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al obtener la lista de libros.", ex);
            }
        }
        #endregion

        #region ObtenerLibrosPorAutorAsync
        public async Task<IEnumerable<LibroResponseDto>> ObtenerLibrosPorAutorAsync(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                throw new KeyNotFoundException("Debe proporcionar un nombre de autor válido.");

            try
            {
                // Búsqueda insensible a mayúsculas
                var libros = await _context.Books
                    .Include(l => l.Autor)
                    .Where(l => l.Autor.Nombre.ToLower() == autor.ToLower())
                    .Select(l => new LibroResponseDto
                    {
                        LibroId = l.Libro_id,
                        Titulo = l.Titulo,
                        AnioPublicacion = l.Año_publicacion,
                        Genero = l.Genero ?? "No especificado",
                        Autor = l.Autor.Nombre
                    })
                    .ToListAsync();

                if (!libros.Any())
                    throw new Exception($"No se encontraron libros para el autor '{autor}'.");

                return libros;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al consultar la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al obtener los libros por autor.", ex);
            }
        }
        #endregion

        #region ObtenerLibroPorTituloAsync
        public async Task<IEnumerable<LibroResponseDto>> ObtenerLibroPorTituloAsync(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío.");

            try
            {
                // Búsqueda parcial
                var libros = await _context.Books
                    .Include(l => l.Autor)
                    .Where(l => EF.Functions.Like(l.Titulo, $"%{titulo}%"))
                    .Select(l => new LibroResponseDto
                    {
                        LibroId = l.Libro_id,
                        Titulo = l.Titulo,
                        AnioPublicacion = l.Año_publicacion,
                        Genero = l.Genero ?? "No especificado",
                        Autor = l.Autor != null ? l.Autor.Nombre : "Desconocido"
                    })
                    .ToListAsync();

                if (libros.Count == 0)
                    throw new KeyNotFoundException("No se ha encontrado ningún libro con el título dado.");

                return libros;
            }
            catch (DbUpdateException ex)
            {
                // Error en la base de datos
                throw new InvalidOperationException("Error al consultar la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Cualquier otro error
                throw new Exception("Error inesperado al buscar libros por título.", ex);
            }
        }
        #endregion

        #region CrearLibroAsync
        public async Task<LibroResponseDto> CrearLibroAsync(LibroDto dto)
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
        #endregion

        #region ActualizarLibroAsync
        public async Task<LibroResponseDto> ActualizarLibroAsync(int id, LibroDto dto)
        {
            var libro = await _context.Books.FindAsync(id);
            if (libro == null)
                throw new KeyNotFoundException($"No se encontró el libro con ID {id}");

            // Actualizar campos
            libro.Titulo = dto.Titulo ?? libro.Titulo;
            libro.Año_publicacion = dto.AnioPublicacion;
            libro.Genero = dto.Genero ?? libro.Genero;

            // Si se actualiza el autor
            var autor = await _context.Authors.FindAsync(dto.AutorId);
            if (autor == null)
                throw new KeyNotFoundException($"No se encontró el autor con ID {dto.AutorId}");
            libro.Autor_id = dto.AutorId;

            await _context.SaveChangesAsync();

            var autorNombre = await _context.Authors
                .Where(a => a.Autor_id == libro.Autor_id)
                .Select(a => a.Nombre)
                .FirstOrDefaultAsync();

            return new LibroResponseDto
            {
                LibroId = libro.Libro_id,
                Titulo = libro.Titulo,
                AnioPublicacion = libro.Año_publicacion,
                Autor = libro.Autor.Nombre,
                Genero = libro.Genero ?? "Genero Desconocido"
            };
        }
        #endregion

        #region EliminarLibroAsync
        public async Task<bool> EliminarLibroAsync(int id)
        {
            var libro = await _context.Books.FindAsync(id);
            if (libro == null)
                throw new KeyNotFoundException($"No se encontró el libro con ID {id}");

            _context.Books.Remove(libro);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
