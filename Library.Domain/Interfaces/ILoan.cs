using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface ILoan
    {
        int Prestamo_id { get; set; }
        int Libro_id { get; set; }
        DateTime Fecha_prestamo { get; set; }
        DateTime? Fecha_devolucion { get; set; }
    }
}