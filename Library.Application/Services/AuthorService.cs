using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Common.Dto;
using Library.Common.Dto.Autores;
using Library.Infrastructure.Data;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Library.Application.Interfaces;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryDbContext _context;

        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }

        #region GetAllAuthorsAsync
        public async Task<List<AuthorResponse>> GetAllAuthorsAsync()
        {
            try
            {
                var autores = await _context.Authors
                    .Include(a => a.Books)
                    .Select(a => new AuthorResponse
                    {
                        Autor_id = a.Autor_id,
                        Nombre = a.Nombre,
                        Nacionalidad = a.Nacionalidad,
                        Books = a.Books.Select(b => new LibroResponseDto
                        {
                            LibroId = b.Libro_id,
                            Titulo = b.Titulo,
                            Genero = b.Genero!,
                            AnioPublicacion = b.Año_publicacion
                        }).ToList()
                    })
                    .ToListAsync();

                // Si no hay autores, devolvemos una respuesta vacía
                if (autores == null || autores.Count == 0)
                {
                    throw new KeyNotFoundException("No se encontraron autores registrados.");
                }

                return autores;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al procesar la solicitud.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los autores.", ex);
            }
        }
        #endregion

        #region GetAuthorByIdAsync
        public async Task<AuthorResponse> GetAuthorByIdAsync(int authorId)
        {
            try
            {
                var autor = await _context.Authors
                    .Include(a => a.Books)
                    .Where(a => a.Autor_id == authorId)
                    .Select(a => new AuthorResponse
                    {
                        Autor_id = a.Autor_id,
                        Nombre = a.Nombre,
                        Nacionalidad = a.Nacionalidad,
                        Books = a.Books.Select(b => new LibroResponseDto
                        {
                            Autor = a.Nombre,
                            LibroId = b.Libro_id,
                            Titulo = b.Titulo,
                            Genero = b.Genero!,
                            AnioPublicacion = b.Año_publicacion
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (autor == null)
                {
                    throw new KeyNotFoundException("El autor no ha sido encontrado");
                }

                return autor;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"No se encontró el autor con ID {authorId}.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al procesar la solicitud.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el autor.", ex);
            }
        }
        #endregion

        #region CreateAuthorAsync
        public async Task<CreateAuthorDto> CreateAuthorAsync(AuthorDto authorDto)
        {
            try
            {
                var nuevoAutor = new Author
                {
                    Nombre = authorDto.Nombre,
                    Nacionalidad = authorDto.Nacionalidad,
                    createdAt = DateTime.UtcNow,
                    updatedAt = DateTime.UtcNow
                };

                _context.Authors.Add(nuevoAutor);
                await _context.SaveChangesAsync();

                // Creamos el objeto de salida con el ID generado por EF
                var response = new CreateAuthorDto
                {
                    AuthorId = nuevoAutor.Autor_id,
                    Nombre = nuevoAutor.Nombre,
                    Nacionalidad = nuevoAutor.Nacionalidad
                };

                return response;
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Error al guardar el autor en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el autor.", ex);
            }
        }
        #endregion

        #region UpdateAuthorAsync
        public async Task<UpdateAuthorDto> UpdateAuthorAsync(int authorId, UpdateAuthorDto authorDto)
        {
            try
            {
                var autorExistente = await _context.Authors.FindAsync(authorId);
                if (autorExistente == null)
                {
                    throw new KeyNotFoundException($"No se encontró el autor con ID {authorId}.");
                }

                autorExistente.Nombre = authorDto.Nombre;
                autorExistente.Nacionalidad = authorDto.Nacionalidad;

                _context.Authors.Update(autorExistente);
                await _context.SaveChangesAsync();

                return authorDto;
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Error al actualizar el autor en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el autor.", ex);
            }
        }
        #endregion

        #region DeleteAuthorAsync
        public async Task<bool> DeleteAuthorAsync(int authorId)
        {
            try
            {
                var autorExistente = await _context.Authors
                    .Include(a => a.Books)
                    .ThenInclude(b => b.Loans)
                    .FirstOrDefaultAsync(a => a.Autor_id == authorId);

                if (autorExistente == null)
                    throw new KeyNotFoundException($"No se encontró el autor con ID {authorId}.");

                _context.Authors.Remove(autorExistente);
                await _context.SaveChangesAsync();

                        return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error al eliminar el autor en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al eliminar el autor.", ex);
            }
        }
        #endregion
    }
}
