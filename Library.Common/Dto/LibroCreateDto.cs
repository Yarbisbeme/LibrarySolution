namespace Library.Common.Dtos
{
    public class LibroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public int AutorId { get; set; }
    }
}
