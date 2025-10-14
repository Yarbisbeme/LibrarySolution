using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Common.Dto.Autores
{
    public class AuthorResponse
    {
        public string Nombre { get; set; } = null!;
        public string Nacionalidad { get; set; } = null!;
        public List<LibroResponseDto> Books { get; set; } = new List<LibroResponseDto>();
    }
}