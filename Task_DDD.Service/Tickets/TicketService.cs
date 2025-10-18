using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Application.RepositoryContracts.Tickets;
using Task_DDD.Application.ServiceContracts.Tickets;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Tickets;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;
using Task_DDD.Service.Base;

namespace Task_DDD.Service.Tickets
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IUserAuthorizedService _userAuthorizedService;
        public TicketService(IUserAuthorizedService userAuthorizedService,
             ITicketRepository ticketRepository,
             IMapper mapper) : base(userAuthorizedService)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _userAuthorizedService = userAuthorizedService;
        }

        public async Task<List<GetAllTicketDto>> GetAllTicketListAsync()
        {

            return await _ticketRepository.GetQueryable(disableMaxRowLimit: true)
                .Include(t => t.User)
                .Include(y => y.Employee)
                .Select(s => new GetAllTicketDto
                {
                    guid = s.Id,
                    AssignedToUser = s.User.FullName,
                    TicketPriorityType = s.TicketPriorityTypeEnum,
                    TicketStatusType = s.TicketStatusTypeEnum,
                    CreatedAt = s.CreatedAt,
                    CreatedByUser = s.Employee.FullName,
                    Description = s.Description,
                    Title = s.Title

                }).ToListAsync();

        }

        public async Task ChangeTicketStatusAsync(Guid ticketId, ChangeTicketStatusDto dto)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);

            ticket.UpdateTicketStatus(dto.status);

            await _ticketRepository.Update(ticket);
        }

        public async Task<Guid> CreateTicketAsync(CreateTicketDto dto)
        {
            if (string.IsNullOrEmpty(dto.Title))
                throw new BusinessException(string.Format(ErrorMessages.TitleIsRequired, dto.Title));
            var userid = _userAuthorizedService.UserId;


            EnumUtility.ValidationEnumDefined(typeof(TicketPriorityTypeEnum), dto.TicketPriorityType, "  Status of Tickets Priority ");

            Ticket ticket;
            ticket = Ticket.CreateTicket(dto.Title, dto.Description, userid, dto.AssignedToUserId, dto.TicketPriorityType);

            await _ticketRepository.Add(ticket);

            return ticket.Id;
        }

        public async Task<List<GetAllTicketDto>> CurrentUserTicketListAsync()
        {
            var currentUserId = _userAuthorizedService.UserId;

            return await _ticketRepository.GetQueryable(disableMaxRowLimit: true)
                .Include(t => t.User)
                .Include(y => y.Employee)
                .Where(w => w.CreatedByUserId == currentUserId)
                .Select(s => new GetAllTicketDto
                {
                    guid = s.Id,
                    AssignedToUser = s.User.FullName,
                    TicketPriorityType = s.TicketPriorityTypeEnum,
                    TicketStatusType = s.TicketStatusTypeEnum,
                    CreatedAt = s.CreatedAt,
                    CreatedByUser = s.Employee.FullName,
                    Description = s.Description,
                    Title = s.Title

                }).ToListAsync();
        }

        public async Task DeleteTicketAsync(Guid ticketId)
        {
            if (ticketId == Guid.Empty) throw new BusinessException(ErrorMessages.GuidNotValid);

            var ticket = await _ticketRepository.GetAsync(ticketId);

            if (ticket == null) throw new BusinessException(ErrorMessages.TicketNotFound);

            await _ticketRepository.Delete(ticket);

        }

        public async Task<GetAllTicketDto> GetDetailTicketAsync(Guid ticketId)
        {

            var ticket= await _ticketRepository.GetQueryable(disableMaxRowLimit: true)
                .Include(t => t.User)
                .Include(y => y.Employee)
                .Where(w => w.Id == ticketId)
                .Select(s => new GetAllTicketDto
                {
                    guid = s.Id,
                    AssignedToUser = s.User.FullName,
                    TicketPriorityType = s.TicketPriorityTypeEnum,
                    TicketStatusType = s.TicketStatusTypeEnum,
                    CreatedAt = s.CreatedAt,
                    CreatedByUser = s.Employee.FullName,
                    Description = s.Description,
                    Title = s.Title

                }).FirstOrDefaultAsync();
            if (ticket == null) throw new BusinessException(ErrorMessages.TicketNotFound);
            return ticket;

        }

        public async Task UpdateTicketAsync(Guid ticketId, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);

            ticket.UpdateTicket(dto.Title, dto.Description, dto.AssignedToUserId, dto.TicketPriorityTypeEnum);

            await _ticketRepository.Update(ticket);
        }
    }
}
