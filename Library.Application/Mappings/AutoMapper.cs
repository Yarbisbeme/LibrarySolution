using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.autor_nombre, opt => opt.MapFrom(src => src.Autor!.nombre));
            CreateMap<CreateBookRequest, Book>();
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.titulo_libro, opt => opt.MapFrom(src => src.Book!.titulo))
                .ForMember(dest => dest.autor_nombre, opt => opt.MapFrom(src => src.Book!.Autor!.nombre));
        }
    }
}
