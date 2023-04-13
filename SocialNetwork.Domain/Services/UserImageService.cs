using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Services
{
    public class UserImageService
    {
        private readonly IUserImagesRepository _userImageRepository;
        public UserImageService(IUserImagesRepository userImageRepository)
        {
            _userImageRepository = userImageRepository;
        }

        public async Task<ICollection<UserImage>> GetAllByUserId(Guid userId)
        {
            return await _userImageRepository.GetAllByUserId(userId);
        }

        public async Task<UserImage> GetById(Guid id)
        {
            return await _userImageRepository.GetById(id);
        }

        public async Task<bool> AddUserImage(UserImage userImage)
        {
            int count = await _userImageRepository.Add(userImage);
            if (count == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateUserImage(UserImage userImage)
        {
            int count = await _userImageRepository.Update(userImage);
            if (count == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUserImage(Guid id)
        {
            var userImage = await _userImageRepository.GetById(id);
            int count = await _userImageRepository.Remove(userImage);
            if (count == 0)
            {
                return false;
            }
            return true;
        }

    }
}
