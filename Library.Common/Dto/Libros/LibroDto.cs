namespace Library.Common.Dto
{
    public class LibroDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public int AutorId { get; set; }
    }
}
