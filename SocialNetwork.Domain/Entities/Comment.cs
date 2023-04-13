using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }        
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public Post Post { get; set; }
    }
}
