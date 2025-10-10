namespace Library.Application.DTOs
{
    public class LoanDto
    {
        public int prestamo_id { get; set; }
        public string titulo_libro { get; set; } = null!;
        public string autor_nombre { get; set; } = null!;
        public DateTime fecha_prestamo { get; set; }
        public DateTime? fecha_devolucion { get; set; }
    }
}
