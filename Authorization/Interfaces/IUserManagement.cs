using Authorization.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Interfaces
{
    public interface IUserManagement
    {
        UserDTO VerifyLogin(string userName, string password);
        Task<UserDTO> CreateUser(string username, string password, string email);
    }
}