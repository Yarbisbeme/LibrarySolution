using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface ILoan
    {
        int prestamo_id { get; set; }
        int libro_id { get; set; }
        DateTime fecha_prestamo { get; set; }
        DateTime? fecha_devolucion { get; set; }
    }
}