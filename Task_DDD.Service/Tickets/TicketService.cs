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
             IMapper mapper,
        ILogger logger) : base(userAuthorizedService, logger)
        {
            _ticketRepository = ticketRepository; 
            _mapper = mapper;
            _userAuthorizedService = userAuthorizedService;
        }

        public async Task<List<GetAllTicketDto>> AllTicketListAsync()
        {           
            return _mapper.Map<List<GetAllTicketDto>>(await _ticketRepository.GetAllAsync());         

        }

        public async Task ChangeTicketAsync(Guid ticketId, TicketStatusTypeEnum dto)
        {
            var ticket= await _ticketRepository.GetAsync(ticketId);

            ticket.UpdateTicketStatus(dto);

            await _ticketRepository.Update(ticket);
        }

        public async Task<Guid> CreateTicketAsync(CreateTicketDto dto)
        {
            if (string.IsNullOrEmpty(dto.Title))
                throw new BusinessException(string.Format(ErrorMessages.TitleIsRequired, dto.Title));

            if (dto.AssignedToUserId == Guid.Empty)
                throw new BusinessException(ErrorMessages.AssignedToUserId);

            EnumUtility.ValidationEnumDefined(typeof(TicketPriorityTypeEnum), dto.TicketPriorityType, "  Status of Tickets Priority ");

            Ticket ticket;
            ticket = Ticket.CreateTicket(dto.Title, dto.Description, dto.AssignedToUserId, dto.TicketPriorityType);

            await _ticketRepository.Add(ticket);

            return ticket.Id;
        }

        public async Task<List<GetAllTicketDto>> CurrentUserTicketListAsync()
        {
            var curentUserId = _userAuthorizedService.UserId;

            return _mapper.Map<List<GetAllTicketDto>>(await _ticketRepository.GetQueryable(disableMaxRowLimit: true)
                    .Where(w => w.CreatedByUserId == curentUserId).ToListAsync());
        }
        public async Task DeleteLibraryAsync(Guid ticketId)
        {
            if (ticketId == Guid.Empty) throw new BusinessException(ErrorMessages.GuidNotValid);

            var ticket = await _ticketRepository.GetAsync(ticketId);

            if (ticket == null) throw new BusinessException(ErrorMessages.TicketNotFound);

           await _ticketRepository.Delete(ticket);

        }

        public async Task<GetAllTicketDto> GetDetailTicketAsync(Guid ticketId)
        {
            return _mapper.Map<GetAllTicketDto>(await _ticketRepository.GetAsync(ticketId));
        }

        public async Task UpdateTicketAsync(Guid ticketId, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetAsync(ticketId);

            ticket.UpdateTicket(dto.Title , dto.Description , dto.AssignedToUserId,dto.TicketPriorityTypeEnum);

           await _ticketRepository.Update(ticket);
        }
    }
}
