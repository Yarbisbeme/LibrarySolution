
namespace Library.Common.Dto
{
    public class LoanResponse
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public DateTime Fecha_Prestamo { get; set; }
        public DateTime? Fecha_Devolucion { get; set; }
    }
}