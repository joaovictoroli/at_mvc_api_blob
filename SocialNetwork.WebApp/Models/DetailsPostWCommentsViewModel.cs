using SocialNetwork.Domain.Entities;

namespace SocialNetwork.WebApp.Models
{
    public class DetailsPostWCommentsViewModel
    {
        public Post post { get; set; }
        public List<Comment> Comment { get; set; }
        public Guid UserId { get; set; } 
        public bool HasLink { get; set; }
    }
}
