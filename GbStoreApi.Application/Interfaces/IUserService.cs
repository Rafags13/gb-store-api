﻿using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Application.Interfaces
{
    public interface IUserService
    {
        ResponseDto<IEnumerable<DisplayUserDto>> GetAll();
        ResponseDto<DisplayUserDto> GetById(int id);
        ResponseDto<DisplayUserDto> GetCurrentInformations();
        ResponseDto<UserType> GetUserRole();
        ResponseDto<bool> Update(UpdateUserDto updateUserDto);
        ResponseDto<User> GetByCredentials(SignInDto signInDto);
    }
}
