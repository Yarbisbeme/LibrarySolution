using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthor
    {
        int autor_id { get; set; }
        string nombre { get; set; }
        string nacionalidad { get; set; }

    }
}