using Domain.Interface;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public void CreateUser(User user) => this.Create(user);

        public void DeleUser(User user) => this.Delete(user);

        public async Task<User> GetByUsernameAsync(string username, bool track)
        {
            return await this.FindByCondition(usr => usr.UserName == username, track).FirstOrDefaultAsync();
        }

        public void UpdateUser(User user) => this.Update(user);

        public async Task SaveAsync() => _context.SaveChangesAsync();

        public async Task<User> GetUserByEmail(string email, bool track)
        {
            return await this.FindByCondition(usr => usr.Email == email, track).FirstOrDefaultAsync();
        }
    }
}
