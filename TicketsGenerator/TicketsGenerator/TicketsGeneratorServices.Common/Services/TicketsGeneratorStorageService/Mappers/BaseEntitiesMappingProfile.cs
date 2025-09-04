using AutoMapper;
using TicketsGeneratorServices.Db.Entities;


namespace TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService.Mappers
{
    public class BaseEntitiesMappingProfile : Profile
    {
        public BaseEntitiesMappingProfile()
        {
            CreateMap<MyAwesomeProductBase, MyAwesomeProduct>();
        }
    }
}