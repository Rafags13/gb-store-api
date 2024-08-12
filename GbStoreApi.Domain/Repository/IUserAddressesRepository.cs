﻿using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface IUserAddressesRepository : IGenericRepository<UserAddress>
    {
        public Guid? GetUserAddressIdByUserAndZipCode(string zipCode, int userId); 
    }
}
