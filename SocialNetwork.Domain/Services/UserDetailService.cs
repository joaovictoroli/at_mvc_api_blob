using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Services
{
    public class UserDetailService
    {
        private readonly IUserDetailsRepository _userDetailsRepository;
        public UserDetailService(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<bool> UserDetailsEmpty()
        {
            var count = _userDetailsRepository.Empty();
            if (count == 0)
            {
                return true;
            }
            return false;
        }

        public async Task<ICollection<UserDetail>> GetAll()
        {
            return await _userDetailsRepository.GetAll();
        }

        public async Task<UserDetail> GetById(Guid id)
        {
            return await _userDetailsRepository.GetById(id);
        }

        public async Task<bool> AddUserDetail(UserDetail userDetail)
        {
            int count = await _userDetailsRepository.Add(userDetail);
            if (count == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateUserDetail(UserDetail userDetail)
        {
            int count = await _userDetailsRepository.Update(userDetail);
            if (count == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUserDetail(UserDetail userDetail)
        {
            int count = await _userDetailsRepository.Remove(userDetail);
            if (count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
