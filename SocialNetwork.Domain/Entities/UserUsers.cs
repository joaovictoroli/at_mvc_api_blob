using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities
{
    public class UserUsers
    {
        public Guid UserId { get; set; }
        public UserDetail User { get; set; }
        public Guid User2Id { get; set; }
        public UserDetail User2 { get; set; }

        public UserUsers(Guid userId, Guid user2Id)
        {
            UserId = userId;
            User2Id = user2Id;
        }
    }
}
