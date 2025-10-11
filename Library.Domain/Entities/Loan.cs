using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public class Loan : BaseEntity, ILoan
    {
        public int Prestamo_id { get; set; }
        public int Libro_id { get; set; }
        public DateTime Fecha_prestamo { get; set; }
        public DateTime? Fecha_devolucion { get; set; }

        public required Book Book { get; set; }
    }
}