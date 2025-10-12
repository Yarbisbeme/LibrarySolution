using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthor
    {
        int Autor_id { get; set; }
        string Nombre { get; set; }
        string Nacionalidad { get; set; }

    }
}