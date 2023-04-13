using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public UserDetail UserDetail { get; set; }
        public string Content { get; set; }
        public ICollection<Comment> Comments { get; set;}
    }
}
