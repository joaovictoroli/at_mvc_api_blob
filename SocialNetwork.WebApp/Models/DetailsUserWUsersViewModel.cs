using SocialNetwork.Domain.Entities;

namespace SocialNetwork.WebApp.Models
{
    public class DetailsUserWUsersViewModel
    {
        public UserDetail userDetail { get; set; }
        public bool HasLinkAlready { get; set; }
        public Guid UserId { get; set; }
    }
}
