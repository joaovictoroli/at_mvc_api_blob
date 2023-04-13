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
    public class CommentRepository : ICommentRepository
    {
        private readonly SocialNetworkDbContext _context;

        public CommentRepository(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Comment comment)
        {
            _context.Comments.Add(comment);
            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Comment>> GetAll()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<List<Comment>> GetAllLinkedComment(Guid PostId)
        {
            var list = await _context.Comments.Where(x => x.PostId == PostId).ToListAsync();
            return list;
        }

        public async Task<Comment> GetById(Guid? id)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == id);
        }

        public async Task<int> Remove(Comment comment)
        {
            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }

        public async Task<Comment> Update(Comment comment)
        {       
            var updatedComment = await GetById(comment.CommentId);
            updatedComment.CommentText = comment.CommentText;
            await _context.SaveChangesAsync();
            return updatedComment;
        }

        public async Task<int> DeleteAllRemainLinkedComments(Guid UserId)
        {
            var toDelete = await _context.Comments.Where(x => x.UserId== UserId).ToListAsync();
            _context.Comments.RemoveRange(toDelete);
            return await _context.SaveChangesAsync();
        }
    }
}
