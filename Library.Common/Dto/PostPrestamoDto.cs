using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Common.Dto
{
    public class PostPrestamoDto
    {
        public int BookId { get; set; }
        public DateTime Fecha_Prestamo { get; set; }
        public DateTime Devolucion_Prestamo { get; set; }
    }
}