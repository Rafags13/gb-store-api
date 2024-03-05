using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
