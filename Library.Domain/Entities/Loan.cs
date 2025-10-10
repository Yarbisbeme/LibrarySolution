using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Common;

namespace Library.Domain.Entities
{
    public class Loan : BaseEntity
    {
        public int prestamo_id { get; set; }
        public int libro_id { get; set; }
        public DateTime fecha_prestamo { get; set; }
        public DateTime? fecha_devolucion { get; set; }

        public Book? Book { get; set; }
    }
}