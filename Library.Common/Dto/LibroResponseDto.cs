namespace Library.Common.Dtos
{
    public class LibroResponseDto
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;   // nombre del autor
        public int AutorId { get; set; }                     // id del autor (nuevo)
        public int AnioPublicacion { get; set; }
    }
}
