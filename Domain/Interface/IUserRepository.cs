using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username, bool track);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleUser(User user);
        Task<User> GetUserByEmail(string email, bool track);
    }
}
