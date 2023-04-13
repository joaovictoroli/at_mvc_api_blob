using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.Infra.Context;

namespace SocialNetwork.Infra.Repositories
{
    public class UserDetailRepository : IUserDetailsRepository
    {
        private readonly SocialNetworkDbContext _context;

        public UserDetailRepository(SocialNetworkDbContext context)
        {
            _context= context;
        }
        public async Task<ICollection<UserDetail>> GetAll()
        {
            return await _context.UserDetails.ToListAsync();
        }

        public int Empty()
        {
            int count = _context.UserDetails.Count();
            return count;
        }

        public async Task<UserDetail> GetById(Guid id)
        {
            return await _context.UserDetails.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<int> Add(UserDetail userDetail)
        {
            _context.UserDetails.Add(userDetail);
            return _context.SaveChanges();
        }

        public async Task<int> Update(UserDetail userDetail)
        {
            var updatedUserDetail = await GetById(userDetail.UserId);
            updatedUserDetail.Name = userDetail.Name;
            updatedUserDetail.ImageUrl = userDetail.ImageUrl;
            return _context.SaveChanges();
        }
        public async Task<int> Remove(UserDetail userDetail)
        {
            _context.UserDetails.Remove(userDetail);
            return _context.SaveChanges();
        }
    }
}
