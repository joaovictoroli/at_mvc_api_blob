using SocialNetwork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Interfaces
{
    public interface IPostRepository
    {
        public Task<ICollection<Post>> GetAll();
        public Task<Post> GetById(Guid? postId);
        public Task<int> Add(Post post);
        public Task<int> Remove(Post post);
        public Task<Post> Update(Post post);
        public Task<int> DeleteAllByUserId(Guid UserId);
        public Task<List<Post>> GetAllByUserId(Guid UserId);
    }
}
