using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBook
    {
        int libro_id { get; set; }
        string titulo { get; set; }
        int autor_id { get; set; }
        int año_publicacion { get; set; }
        string? genero { get; set; }
    }
}