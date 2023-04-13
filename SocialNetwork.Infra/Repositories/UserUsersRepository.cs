using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infra.Repositories
{
    public class UserUsersRepository : IUserUsersRepository
    {
        private readonly SocialNetworkDbContext _context;
        public UserUsersRepository(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<UserUsers>> GetLinkedUserUsers(Guid UserId)
        {
            var list = await _context.UserUsers.Where(x => x.UserId == UserId || x.User2Id == UserId).ToListAsync();
            return list;
        }

        public async Task<ICollection<UserUsers>> GetAll()
        { 
            return await _context.UserUsers.ToListAsync();
        }

        public async Task<bool> IsLinkedUser(Guid UserId, Guid UserId2)
        {
            if(await _context.UserUsers.Where(x => x.UserId == UserId && x.User2Id == UserId2).AnyAsync())
            {
                return true;
            }

            if(await _context.UserUsers.Where(x => x.UserId == UserId2 && x.User2Id == UserId).AnyAsync())
            {
                return true;
            }
            return false;
        }
        public async Task<int> Add(UserUsers userUser)
        {
            _context.UserUsers.Add(userUser);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(UserUsers userUser)
        {
            try
            {
                var user = await _context.UserUsers.FirstOrDefaultAsync(x => x.UserId == userUser.UserId && x.User2Id == userUser.User2Id);
                _context.UserUsers.Remove(user);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                var user = await _context.UserUsers.FirstOrDefaultAsync(x => x.UserId == userUser.User2Id && x.User2Id == userUser.UserId);
                _context.UserUsers.Remove(user);
                return await _context.SaveChangesAsync();
            }
        }

        public async Task<Guid> RemoveRelation(UserUsers userUser)
        {
            try
            {
                var user = await _context.UserUsers.FirstOrDefaultAsync(x => x.UserId == userUser.UserId && x.User2Id == userUser.User2Id);
                _context.UserUsers.Remove(user);
                await _context.SaveChangesAsync();
                return user.UserId;
            }
            catch
            {
                var user = await _context.UserUsers.FirstOrDefaultAsync(x => x.UserId == userUser.User2Id && x.User2Id == userUser.UserId);
                _context.UserUsers.Remove(user);
                await _context.SaveChangesAsync();
                return user.User2Id;
            }
        }
    }
}
