﻿using ExchangeStuff.Core.Repositories;
using ExchangeStuff.Core.Uows;
using ExchangeStuff.Service.Maps;
using ExchangeStuff.Service.Models.Users;
using ExchangeStuff.Service.Services.Interfaces;

namespace ExchangeStuff.Service.Services.Impls
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _userRepository = _uow.UserRepository;
        }

        public async Task<UserViewModel> GetUser(Guid id)
            => AutoMapperConfig.Mapper.Map<UserViewModel>(await _userRepository.GetOneAsync(x => x.Id == id, "PermissionGroups,Campus,UserBalance"));

        public async Task<List<UserViewModel>> GetUsers(string? name = null, string? email = null, string? username = null, int? pageIndex = null, int? pageSize = null)
            => AutoMapperConfig.Mapper.Map<List<UserViewModel>>(await _userRepository.GetUserFilter(name, email, username, "PermissionGroups,Campus,UserBalance", pageIndex, pageSize));
    }
}