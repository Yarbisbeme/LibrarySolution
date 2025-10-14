
namespace Library.Application.DTOs;

    public class PrestamoNoDevueltoDto
    {
        public int AutorId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
    }
