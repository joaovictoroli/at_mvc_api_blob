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
    public class UserImageRepository : IUserImagesRepository
    {
        private readonly SocialNetworkDbContext _context;
        public UserImageRepository(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<UserImage>> GetAll()
        {
            return await _context.UserImages.ToListAsync();
        }

        public async Task<ICollection<UserImage>> GetAllByUserId(Guid UserId)
        {           
            return await _context.UserImages.Where(x => x.UserId == UserId).ToListAsync();
        }

        public int Empty()
        {
            int count = _context.UserImages.Count();
            return count;
        }

        public async Task<UserImage> GetById(Guid id)
        {
            return await _context.UserImages.FirstOrDefaultAsync(x => x.ImageId == id);
        }

        public async Task<int> Add(UserImage userImage)
        {
            _context.UserImages.Add(userImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(UserImage userImage)
        {
            _context.UserImages.Remove(userImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(UserImage userImage)
        {
            var updatedUserImage = await GetById(userImage.ImageId);
            updatedUserImage.ImageUrl = userImage.ImageUrl;
            return await _context.SaveChangesAsync();
        }
    }
}
