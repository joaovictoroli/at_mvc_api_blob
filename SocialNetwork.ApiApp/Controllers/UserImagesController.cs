using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Services;
using static System.Net.Mime.MediaTypeNames;

namespace SocialNetwork.ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserImagesController : Controller
    {
        private readonly UserImageService _service;
        public UserImagesController(UserImageService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("UserDetails/{userId}")]
        public async Task<IEnumerable<UserImage>> GetAll(Guid userId)
        {
            return await _service.GetAllByUserId(userId);
        }

        [HttpGet("{imageId}")]
        public async Task<ActionResult<UserImage>> GetById(Guid imageId)
        {
            return await _service.GetById(imageId);
        }

        [HttpPost]
        public async Task<ActionResult<UserImage>> PostUserImage(UserImage userImage)
        {
            await _service.AddUserImage(userImage);
            return userImage;
        }

        [HttpPut("{imageId}")]
        public async Task<ActionResult<UserImage>> PutUserImage(UserImage userImage)
        {
            var existingUserImage = await _service.GetById(userImage.ImageId);

            if (existingUserImage == null)
            {
                return NotFound();
            }
            var image = await _service.UpdateUserImage(userImage);

            return userImage;
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUserImage(Guid? id)
        {
            var userImage = await _service.RemoveUserImage(id.Value);

            if (userImage == null)
            {
                return NotFound();
            }
            return true;
        }
    }
}