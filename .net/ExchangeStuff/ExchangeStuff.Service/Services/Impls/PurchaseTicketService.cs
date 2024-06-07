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
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseTicketRepository _purchaseTicketRepository;

        public PurchaseTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _purchaseTicketRepository = _unitOfWork.PurchaseTicketRepository;
        }

        public Task<ActionResult> CreatePurchaseTicket(CreatePurchaseTicketModel request)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> GetAllPurchaseTicket(int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            ActionResult actionResult = new ActionResult();
            List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
            try
            {
                if (status != null) {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status, orderBy: p => p.OrderBy(p => p.CreatedOn));
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

        public async Task<ActionResult> GetListPurchaseTicketByUserId(Guid userId, int pageSize, int pageIndex, PurchaseTicketStatus status)
        {
            ActionResult actionResult = new ActionResult();
            List<PurchaseTicket> listTicket = new List<PurchaseTicket>();
            try
            {
                if (status != null)
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status && p.UserId == userId, orderBy: p => p.OrderBy(p => p.CreatedOn)); 
                }
                else
                {
                    listTicket = await _purchaseTicketRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId == userId, orderBy: p => p.OrderBy(p => p.CreatedOn));
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
                ticket = await _purchaseTicketRepository.GetOneAsync(predicate: p => p.Id == purchaseTicketId);
                var result = AutoMapperConfig.Mapper.Map<PurchaseTicketViewModel>(ticket);
                return actionResult.BuildResult(result);
            }
            catch (Exception ex)
            {
                return actionResult.BuildError("Server error", HttpStatusCode.InternalServerError);
            }
        }

        public Task<ActionResult> UpdatePurchaseTicket(UpdatePurchaseTicketModel request)
        {
            throw new NotImplementedException();
        }
    }
}
