using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities
{
    public class UserImage
    {
        [Key]
        public Guid ImageId { get; set; }
        public Guid UserId { get; set; }
        public string ImageUrl { get; set; }
    }
}
