using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Common.Dto.Autores
{
    public class CreateAuthorDto
    {
        public int AuthorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Nacionalidad { get; set; } = null!;
    }
}