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

        /// <summary>
        /// Obtiene todos los autores junto con sus libros asociados.
        /// </summary>
        public async Task<ApiResponse<List<AuthorResponse>>> GetAllAuthorsAsync()
        {
            try
            {
                var autores = await _context.Authors
                    .Include(a => a.Books)
                    .Select(a => new AuthorResponse
                    {
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
                    return ApiResponse<List<AuthorResponse>>.ErrorResponse("No se encontraron autores registrados.");
                }

                return ApiResponse<List<AuthorResponse>>.SuccessResponse(autores, "Autores obtenidos correctamente.");
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

        /// <Summary>
        /// Servicio para obtener un autor por su ID
        /// <Summary>
        public async Task<ApiResponse<AuthorResponse>> GetAuthorByIdAsync(int authorId)
        {
            try
            {
                var autor = await _context.Authors
                    .Include(a => a.Books)
                    .Where(a => a.Autor_id == authorId)
                    .Select(a => new AuthorResponse
                    {
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
                    .FirstOrDefaultAsync();

                if (autor == null)
                {
                    return ApiResponse<AuthorResponse>.ErrorResponse($"No se encontró el autor con ID {authorId}.");
                }

                return ApiResponse<AuthorResponse>.SuccessResponse(autor, "Autor obtenido correctamente.");
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

        /// <summary>
        /// Servicio para crear un nuevo autor
        /// </summary>
        public async Task<ApiResponse<CreateAuthorDto>> CreateAuthorAsync(AuthorDto authorDto)
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

                return ApiResponse<CreateAuthorDto>.SuccessResponse(response, "Autor creado correctamente.");
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


        /// <summary>
        /// Servicio para actualizar un autor existente
        /// </summary>
        public async Task<ApiResponse<AuthorDto>> UpdateAuthorAsync(int authorId, AuthorDto authorDto)
        {
            try
            {
                var autorExistente = await _context.Authors.FindAsync(authorId);
                if (autorExistente == null)
                {
                    return ApiResponse<AuthorDto>.ErrorResponse($"No se encontró el autor con ID {authorId}.");
                }

                autorExistente.Nombre = authorDto.Nombre;
                autorExistente.Nacionalidad = authorDto.Nacionalidad;

                _context.Authors.Update(autorExistente);
                await _context.SaveChangesAsync();

                return ApiResponse<AuthorDto>.SuccessResponse(authorDto, "Autor actualizado correctamente.");
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
    
        /// <summary>
        /// Servicio para eliminar un autor por su ID
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteAuthorAsync(int authorId)
        {
            try
            {
                var autorExistente = await _context.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Autor_id == authorId);

                if (autorExistente == null)
                {
                    return ApiResponse<bool>.ErrorResponse($"No se encontró el autor con ID {authorId}.");
                }

                if (autorExistente.Books != null && autorExistente.Books.Any())
                {
                    return ApiResponse<bool>.ErrorResponse("No se puede eliminar el autor porque tiene libros asociados.");
                }

                _context.Authors.Remove(autorExistente);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Autor eliminado correctamente.");
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Error al eliminar el autor en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el autor.", ex);
            }
        }
    }
}
