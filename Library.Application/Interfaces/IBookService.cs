
using Library.Common.Dto;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<LibroResponseDto>> ObtenerLibrosAntesDe2000Async();
        Task<IEnumerable<LibroResponseDto>> ObtenerLibroPorTituloAsync(string titulo);
        Task<IEnumerable<LibroResponseDto>> ObtenerLibrosPorAutorAsync(string autor);
        Task<LibroResponseDto> ActualizarLibroAsync(int id, LibroDto dto);
        Task<LibroResponseDto> CrearLibroAsync(LibroDto dto);
        Task<bool> EliminarLibroAsync(int id);

    }
}
