namespace Library.Application.DTOs
{
    public class CreateBookRequest
    {
        public string titulo { get; set; } = null!;
        public int autor_id { get; set; }
        public int año_publicacion { get; set; }
        public string? genero { get; set; }
    }
}
