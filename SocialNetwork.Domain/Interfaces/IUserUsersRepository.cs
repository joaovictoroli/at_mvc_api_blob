using SocialNetwork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Interfaces
{
    public interface IUserUsersRepository
    {
        public Task<ICollection<UserUsers>> GetLinkedUserUsers(Guid UserId);
        public Task<ICollection<UserUsers>> GetAll();

        public Task<bool> IsLinkedUser(Guid UserId, Guid UserId2);

        Task<int> Add(UserUsers userUser);

        Task<int> Remove(UserUsers userUser);

        Task<Guid> RemoveRelation(UserUsers userUser);
    }
}
