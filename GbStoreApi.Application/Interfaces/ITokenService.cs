using GbStoreApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface ITokenService
    {
        string Generate(UserTokenDto user);
    }
}
