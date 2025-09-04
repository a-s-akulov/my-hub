using AutoMapper;
using TicketsGeneratorServices.Api.DTO.V1.MyAwesomeProducts;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Api.RequestHandlers.V1.MyAwesomeProducts.Mappers
{
    public class MyAwesomeProductDtoMappingProfile : Profile
    {
        public MyAwesomeProductDtoMappingProfile()
        {
            CreateMap<MyAwesomeProductToPut, MyAwesomeProduct>();
        }
    }
}