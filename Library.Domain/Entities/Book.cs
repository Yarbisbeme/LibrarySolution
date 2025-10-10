using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public class Book : BaseEntity, IBook
    {
        public int libro_id { get; set; }
        public string titulo { get; set; } = null!;
        public int autor_id { get; set; }
        public required int año_publicacion { get; set; }
        public string? genero { get; set; }

        public Author? Autor { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }
}