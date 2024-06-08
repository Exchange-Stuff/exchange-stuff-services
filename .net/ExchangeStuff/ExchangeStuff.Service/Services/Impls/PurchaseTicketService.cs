using Azure.Core;
using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.PurchaseTicket;
using ExchangeStuff.Service.Services.Interfaces;
using MediatR;
using System.Net;
using System.Net.NetworkInformation;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PurchaseTicketService : IPurchaseTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseTicketRepository _purchaseTicketRepository;

        public PurchaseTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _purchaseTicketRepository = _unitOfWork.PurchaseTicketRepository;
            _userRepository = _unitOfWork.UserRepository;
        }

        public async Task<ActionResult> CreatePurchaseTicket(CreatePurchaseTicketModel request)
        {
            ActionResult actionResult = new ActionResult();
            var user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(request.UserId));
            if (user == null) 
                return actionResult.BuildError("Not found user", HttpStatusCode.NotFound);
            try
            {
                var ticket = AutoMapperConfig.Mapper.Map<PurchaseTicket>(request);
                await _purchaseTicketRepository.AddAsync(ticket);
                await _unitOfWork.SaveChangeAsync();
                return actionResult.BuildResult();
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ActionResult> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            ActionResult actionResult = new ActionResult();
            List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
            try
            {
                if (status.HasValue) {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status.Equals(status), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return actionResult.BuildResult(result);
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ActionResult> GetListPurchaseTicketByUserId(Guid userId, int pageSize, int pageIndex, PurchaseTicketStatus? status = null!)
        {
            ActionResult actionResult = new ActionResult();
            List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
            try
            {
                if (status.HasValue)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status.Equals(status) && p.UserId.Equals(userId), orderBy: p => p.OrderBy(p => p.CreatedOn)); 
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId.Equals(userId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<PurchaseTicketViewModel>>(listTicket);
                return actionResult.BuildResult(result);
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ActionResult> GetPurchaseTicketDetail(Guid purchaseTicketId)
        {
            ActionResult actionResult = new ActionResult();
            PurchaseTicket ticket = new PurchaseTicket();
            try
            {
                ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id.Equals(purchaseTicketId));
                var result = AutoMapperConfig.Mapper.Map<PurchaseTicketViewModel>(ticket);
                return actionResult.BuildResult(result);
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ActionResult> UpdatePurchaseTicket(UpdatePurchaseTicketModel request)
        {
            var actionResult = new ActionResult();
            var ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id.Equals(request.Id));
            if (ticket == null) return actionResult.BuildError("Not found ticket!", HttpStatusCode.NotFound);

            try
            {
                ticket.Status = request.Status;
                _purchaseTicketRepository.Update(ticket);
                await _unitOfWork.SaveChangeAsync();
                return actionResult.BuildResult();
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
