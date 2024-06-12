using ExchangeStuff.Core.Entities;
using ExchangeStuff.Core.Enums;
using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.CurrentUser.Users;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.FinancialTickets;
using ExchangeStuff.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using ExchangeStuff.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Azure.Messaging;
using AutoMapper;

namespace ExchangeStuff.Service.Services.Impls
{
    public class FinancialTicketService : IFinancialTicketService
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IFinancialTicketsRepository _financialTicketsRepository;

        public FinancialTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _financialTicketsRepository = _unitOfWork.FinancialTicketsRepository;
            _userRepository = _unitOfWork.UserRepository;
        }

        public async Task<bool> CreateFinancialTicket(CreateFinancialTicketModel request)
        {
            try
            {
                User user = await _userRepository.GetOneAsync(predicate: u => u.Id.Equals(_identityUser.AccountId));
                FinancialTicket financialTicket = new FinancialTicket
                {
                    Id = Guid.NewGuid(),
                    Amount = request.Amount,
                    UserId = _identityUser.AccountId,
                    IsCredit = true,
                    Status = FinancialTicketStatus.Pending,
                };
                await _financialTicketsRepository.AddAsync(financialTicket);
                var result = await _unitOfWork.SaveChangeAsync();
                return result > 0;

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        
        }

        public async Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
        {
            
            List<FinancialTicket> listTicket = new List<FinancialTicket>();
            try
            {
                if(status != null)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize:  pageSize, pageIndex: pageIndex, predicate: p => p.Status == status, orderBy: p=>p.OrderBy(p=> p.CreatedOn));

                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(pageSize: pageSize, pageIndex: pageIndex, orderBy: p=> p.OrderBy(p=>p.CreatedOn));

                }
                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
               return result;

            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId( int pageSize, int pageIndex, FinancialTicketStatus? status = null!)
        {
           
           
            try
            {
                List<FinancialTicket> listTicket = new List<FinancialTicket>(); 
                if (status.HasValue)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status && p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId.Equals(_identityUser.AccountId), orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FinancialTicketViewModel> GetFinancialTicketDetail(Guid financialTicketId)
        {
           
            FinancialTicket ticket = new FinancialTicket();
            try
            {
                ticket = await _financialTicketsRepository.GetOneAsync(predicate: p => p.Id == financialTicketId);
                var result = AutoMapperConfig.Mapper.Map<FinancialTicketViewModel>(ticket);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateFinancialTicket(UpdateFinancialTicketModel request)
        {
            try
            {
                FinancialTicket ticket = await _financialTicketsRepository.GetOneAsync(predicate: p=> p.Id.Equals(request.Id));
                if (ticket == null)
                {
                    throw new Exception("Not found ticket");



                }
                else
                {
                    ticket.Status = request.Status;
                    _financialTicketsRepository.Update(ticket);
                    var result = await _unitOfWork.SaveChangeAsync();
                    return result > 0;
                }

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

        }

        
    }
}
