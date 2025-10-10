namespace Library.Application.DTOs
{
    public class BookDto
    {
        public int libro_id { get; set; }
        public string titulo { get; set; } = null!;
        public int año_publicacion { get; set; }
        public string? genero { get; set; }
        public string autor_nombre { get; set; } = null!;
    }
}
