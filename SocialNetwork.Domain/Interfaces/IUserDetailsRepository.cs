using SocialNetwork.Domain.Entities;
using System.ComponentModel;

namespace SocialNetwork.Domain.IUsuarioRepository.cs
{
    public interface IUserDetailsRepository
    {
        public int Empty();
        public Task<ICollection<UserDetail>> GetAll();
        public Task<UserDetail> GetById(Guid id);
        public Task<int> Add(UserDetail userDetail);
        public Task<int> Update(UserDetail userDetail);
        public Task<int> Remove(UserDetail userDetail);
    }
}
