using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.WebApp.Models;
using System.Security.Claims;

namespace SocialNetwork.WebApp.Controllers
{
    [Authorize]
    public class UserUsersControllers : Controller
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        private readonly IUserUsersRepository _userUsersRepository;

        public UserUsersControllers(IUserDetailsRepository userDetailsRepository, IUserUsersRepository userUsersRepository)
        {
            _userDetailsRepository = userDetailsRepository;
            _userUsersRepository = userUsersRepository;
        }

        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id != null)
            {
                var user = await _userDetailsRepository.GetById(Id.Value);
                var viewModel = new DetailsUserWUsersViewModel();
                viewModel.userDetail = user;
                viewModel.UserId = GetUserId();
                viewModel.HasLinkAlready = await _userUsersRepository.IsLinkedUser(viewModel.UserId, Id.Value);
                return View(viewModel);
            }
            return NotFound();
        }
        public async Task<IActionResult> LinkUserUsers(Guid UserId, Guid UserId2)
        {
            var userUser = new UserUsers(UserId, UserId2);
            await _userUsersRepository.Add(userUser);

            return RedirectToAction("Details", new { Id = UserId2 });
        }

        public async Task<IActionResult> RemoveLink(Guid UserId, Guid UserId2)
        {
            var userUser = new UserUsers(UserId, UserId2);
            await _userUsersRepository.RemoveRelation(userUser);

            // mudar metodo

            return RedirectToAction("Details", new { Id = UserId2 });
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
