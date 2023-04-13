using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Services;
using System.Security.Claims;

namespace SocialNetwork.WebApp.Controllers
{
    [Authorize]
    public class UserImagesController : Controller
    {
        private readonly ApiUserImageService _apiService;

        public UserImagesController(ApiUserImageService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var list = await _apiService.GetAllByUserId(userId);
            return View(list);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _apiService.GetByImageId(id.Value);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        public IActionResult Create()
        {
            UserImage userImage = new UserImage()
            {
                ImageId = Guid.NewGuid(),
                UserId = GetUserId()
            };
            return View(userImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserImage userImage, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                userImage.ImageUrl = await BlobAzure.UploadImage(ImageUrl);
                userImage.UserId = GetUserId();

                await _apiService.AddUserImage(userImage);
                return RedirectToAction(nameof(Index));
            }
            return View(userImage);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage = await _apiService.GetByImageId(id.Value);
            if (userImage == null)
            {
                return NotFound();
            }

            return View(userImage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userImage = await _apiService.GetByImageId(id);
            if (userImage == null)
            {
                return NotFound();
            }
            await _apiService.DeleteUserImage(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage = await _apiService.GetByImageId(id.Value);

            if (userImage == null)
            {
                return NotFound();
            }
            return View(userImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserImage userImage, IFormFile ImageUrl)
        {
            if (ModelState.IsValid && ImageUrl != null)
            {
                var imageToDelete = await _apiService.GetByImageId(userImage.ImageId);
                BlobAzure.DeletePhoto(imageToDelete.ImageUrl);
                userImage.ImageUrl = await BlobAzure.UploadImage(ImageUrl);
                var response = await _apiService.UpdateUserImage(userImage);
                if (response == false)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }        

            return View(userImage);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
