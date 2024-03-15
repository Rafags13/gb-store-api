using GbStoreApi.Domain.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Extensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddCustomJwtConfiguration(
            this AuthenticationBuilder builder,
            ConfigurationManager configurationClass
            )
        {
            builder
                .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                var configuration = new MyConfigurationClass { PrivateKey = configurationClass.GetSection("Configuration").GetValue("PrivateKey", "") ?? "" };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.PrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return builder;
        }
    }
}
