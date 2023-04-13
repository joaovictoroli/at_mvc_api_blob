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
    public class PostRepository : IPostRepository
    {
        private readonly SocialNetworkDbContext _context;

        public PostRepository(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Post>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetById(Guid? id)
        {
            return await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);
        }

        public async Task<int> Add(Post post)
        {
            _context.Posts.Add(post);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Post post)
        {
            _context.Posts.Remove(post);
            return _context.SaveChanges();
        }

        public async Task<Post> Update(Post post)
        {
            var updatedPost = await GetById(post.PostId);
            updatedPost.Title = post.Title;
            updatedPost.Content= post.Content;
            await _context.SaveChangesAsync();
            return updatedPost;
        }

        public async Task<int> DeleteAllByUserId(Guid UserId)
        {
            var linkedPost = await _context.Posts.Where(x => x.UserId == UserId).ToListAsync();
            _context.Posts.RemoveRange(linkedPost);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllByUserId(Guid UserId)
        {
            var listPost = _context.Posts.Where(x => x.UserId == UserId);
            return await listPost.ToListAsync();
        }
    }
}
