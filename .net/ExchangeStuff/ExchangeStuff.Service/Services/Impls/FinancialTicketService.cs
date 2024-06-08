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

namespace ExchangeStuff.Service.Services.Impls
{
    public class FinancialTicketService : IFinancialTicketService
    {
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinancialTicketsRepository _financialTicketsRepository;

        public FinancialTicketService(IIdentityUser<Guid> identityUser, IUnitOfWork unitOfWork)
        {
            _identityUser = identityUser;
            _unitOfWork = unitOfWork;
            _financialTicketsRepository = _unitOfWork.FinancialTicketsRepository;
        }

        public Task<FinancialTicketViewModel> CreateFinancialTicket(CreateFinancialTicketModel request)
        { 
            throw new NotImplementedException();
        
        }

        public async Task<List<FinancialTicketViewModel>> GetAllFinancialTicket(int pageSize, int pageIndex, FinancialTicketStatus status)
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

                throw;
            }

        }

        public async Task<List<FinancialTicketViewModel>> GetFinancialTicketByUserId(Guid userId, int pageSize, int pageIndex, FinancialTicketStatus status)
        {
           
            List<FinancialTicket> listTicket = new List<FinancialTicket>();
            try
            {
                if (status != null)
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.Status == status && p.UserId == userId, orderBy: p => p.OrderBy(p => p.CreatedOn));
                }
                else
                {
                    listTicket = await _financialTicketsRepository.GetManyAsync(
                        pageSize: pageSize, pageIndex: pageIndex, predicate: p => p.UserId == userId, orderBy: p => p.OrderBy(p => p.CreatedOn));
                }

                var result = AutoMapperConfig.Mapper.Map<List<FinancialTicketViewModel>>(listTicket);
                return result;
            }
            catch (Exception ex)
            {
                throw;
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
               throw;
            }
        }

        public Task<UpdateFinancialTicketModel> UpdateFinancialTicket(UpdateFinancialTicketModel request)
        {
            throw new NotImplementedException();
        }

        
    }
}
