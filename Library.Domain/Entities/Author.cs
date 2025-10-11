
using Library.Domain.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public class Author : BaseEntity, IAuthor
    {
        public int Autor_id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Nacionalidad { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}