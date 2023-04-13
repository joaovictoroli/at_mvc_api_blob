using SocialNetwork.Domain.Entities;

namespace SocialNetwork.WebApp.Models
{
    public class IndexPostViewModel
    {
        public List<Post> post { get; set; }
        public Guid userId { get; set; }
    }
}
