using SocialNetwork.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Interfaces
{
    public interface  ICommentRepository
    {
        public Task<ICollection<Comment>> GetAll();
        public Task<Comment> GetById(Guid? comment);
        public Task<int> Add(Comment comment);
        public Task<List<Comment>> GetAllLinkedComment(Guid PostId);
        public Task<int> Remove(Comment comment);
        public Task<Comment> Update(Comment comment);
        public Task<int> DeleteAllRemainLinkedComments(Guid UserId);
    }
}
