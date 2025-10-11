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
        public int Libro_id { get; set; }
        public string Titulo { get; set; } = null!;
        public int Autor_id { get; set; }
        public required int Año_publicacion { get; set; }
        public string? Genero { get; set; }

        public Author Autor { get; set; } = null!;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }
}