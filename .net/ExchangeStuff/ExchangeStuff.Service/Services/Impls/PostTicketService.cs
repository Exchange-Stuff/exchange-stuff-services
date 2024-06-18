using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Repository.Repositories;
using ExchangeStuff.Service.Models.PostTicket;
using ExchangeStuff.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeStuff.Service.Services.Impls
{
    public class PostTicketService : IPostTicketService
    {
        private readonly IPostTicketRepository _postTicketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUser<Guid> _identityUser;

        public PostTicketService(IUnitOfWork unitOfWork, IIdentityUser<Guid> identityUser)
        {
            _unitOfWork = unitOfWork;
            _postTicketRepository = _unitOfWork.PostTicketRepository;
            _identityUser = identityUser;
        }

        public async Task<bool> UpdateStatusAsync(Guid productId, PostTicketStatus status)
        {

            try
            {
                var postTicket = await _postTicketRepository.GetOneAsync(p => p.ProductId == productId);

                if (postTicket == null)
                {
                    throw new Exception("Not found post ticket");
                }

                postTicket.Status = status;

                _postTicketRepository.Update(postTicket);

                await _unitOfWork.SaveChangeAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
