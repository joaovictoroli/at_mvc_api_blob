using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Domain.Entities
{
    public class UserDetail
    {
        [Key]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<UserUsers> UserContacts { get; set; }
        public ICollection<UserUsers> User2Contacts { get; set; }
    }
}
