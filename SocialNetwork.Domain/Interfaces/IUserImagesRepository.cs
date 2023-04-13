using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Domain.Interfaces
{
    public interface IUserImagesRepository
    {
        public int Empty();
        public Task<ICollection<UserImage>> GetAll();
        public Task<ICollection<UserImage>> GetAllByUserId(Guid id);
        public Task<UserImage> GetById(Guid id);
        public Task<int> Add(UserImage userImage);
        public Task<int> Update(UserImage userImage);
        public Task<int> Remove(UserImage userImage);
    }
}
