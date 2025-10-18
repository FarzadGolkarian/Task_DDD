using AutoMapper;
using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Domain.Entity.Tickets;

namespace Task_DDD.Application.ProfileMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Ticket, GetAllTicketDto>()
                .ForMember(s => s.AssignedToUser, f => f.MapFrom(d => d.User.FullName))
                .ForMember(s => s.CreatedByUser, f => f.MapFrom(d => d.Employee.FullName))
                .ReverseMap();

        }
    }
}
