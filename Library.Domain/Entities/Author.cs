using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public class Author : BaseEntity, IAuthor
    {
        public int autor_id { get; set; }
        public string nombre { get; set; } = null!;
        public string nacionalidad { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}