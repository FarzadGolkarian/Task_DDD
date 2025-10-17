using AutoMapper;
using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Domain.Entity.Tickets;

namespace Task_DDD.Application.ProfileMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Ticket, GetAllTicketDto>().ReverseMap();
        }
    }
}
