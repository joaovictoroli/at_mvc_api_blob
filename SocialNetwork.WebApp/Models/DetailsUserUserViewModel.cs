using Microsoft.Build.Framework;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.WebApp.Models
{
    public class DetailsUserUserViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid UserId2 { get; set; }
        public UserDetail userDetail { get; set; }
        public List<UserUsersViewModel> userUsers { get; set; }
        public List<Post> UserPosts { get; set; }
    }
}
