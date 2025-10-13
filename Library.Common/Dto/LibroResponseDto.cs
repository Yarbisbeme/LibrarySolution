namespace Library.Common.Dto
{
    public class LibroResponseDto
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;   // nombre del autor
        public int AnioPublicacion { get; set; }
    }
}
