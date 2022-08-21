using Authorization.DataTransferObjects;
using Authorization.Interfaces;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    public class UserManagement : IUserManagement
    {
        private readonly ICryptography _cryptographyServise;
        private readonly WebDatabaseContext _webDatabaseContext;

        public UserManagement(
            ICryptography cryptographyServise,
            WebDatabaseContext webDatabaseContext)
        {
            _cryptographyServise = cryptographyServise;
            _webDatabaseContext = webDatabaseContext;
        }

        public UserDTO VerifyLogin(string userName, string password)
        {
            var hashedPassword = _cryptographyServise.Hash(password);
            var user = _webDatabaseContext.Users.Where(x => x.UserName == userName && x.PasswordHash == hashedPassword).FirstOrDefault();

            return user == null
                ? null
                : new UserDTO
                {
                    Username = user.UserName,
                    Role = user.Role,
                    Email = user.Email
                };
        }

        public async Task<UserDTO> CreateUser(string username, string password, string email)
        {
            try
            {
                var hashedPassword = _cryptographyServise.Hash(password);

                var user = new User
                {
                    UserName = username,
                    Email = email,
                    PasswordHash = hashedPassword,
                    Role = "User",
                    Created = DateTime.Now,
                    LastSeen = DateTime.Now
                };

                _webDatabaseContext.Users.Add(user);

                await _webDatabaseContext.SaveChangesAsync();

                return new UserDTO { Username = user.UserName, Role = user.Role, Email = user.Email };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
