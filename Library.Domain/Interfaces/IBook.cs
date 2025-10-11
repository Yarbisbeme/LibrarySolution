using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBook
    {
        int Libro_id { get; set; }
        string Titulo { get; set; }
        int Autor_id { get; set; }
        int Año_publicacion { get; set; }
        string? Genero { get; set; }
    }
}