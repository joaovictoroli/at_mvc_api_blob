using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.Services;
using SocialNetwork.Infra.Context;
using SocialNetwork.Infra.Repositories;
using SocialNetwork.WebApp.Models;

namespace SocialNetwork.WebApp.Controllers
{
    [Authorize]
    public class UserDetailsController : Controller
    {
        private readonly ApiUserDetailService _apiService;
        private readonly IUserUsersRepository _userUsersRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public UserDetailsController(ApiUserDetailService apiService, IUserUsersRepository userUsersRepository, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _apiService = apiService;
            _userUsersRepository = userUsersRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        // GET: UserDetails
        public async Task<IActionResult> Index()
        {
            Guid userId = GetUserId();
            UserDetail userDetail = await _apiService.GetById(userId);
            if (userDetail == null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Details", new { id = userId });
            //return View(await _apiService.GetAll());
        }

        // GET: UserDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new DetailsUserUserViewModel();
            viewModel.userDetail = await _apiService.GetById(id.Value);

            if (viewModel.userDetail == null)
            {
                return NotFound();
            }

            var userUsers = await _userUsersRepository.GetLinkedUserUsers(viewModel.userDetail.UserId);
            viewModel.UserPosts = await _postRepository.GetAllByUserId(viewModel.userDetail.UserId);

            viewModel.userUsers = new List<UserUsersViewModel>();
            foreach (var item in userUsers)
            {
                if (item.UserId != id.Value)
                {
                    var user = await _apiService.GetById(item.UserId);
                    var userUserViewModel = new UserUsersViewModel()
                    {
                        UserId = user.UserId,
                        ImageUrl = user.ImageUrl,
                        Name = user.Name
                    };

                    viewModel.userUsers.Add(userUserViewModel);
                }
                else if (item.User2Id != id.Value)
                {
                    var user = await _apiService.GetById(item.User2Id);
                    var userUserViewModel = new UserUsersViewModel()
                    {
                        UserId = user.UserId,
                        ImageUrl = user.ImageUrl,
                        Name = user.Name
                    };

                    viewModel.userUsers.Add(userUserViewModel);
                }
            }

            return View(viewModel);
        }

        // GET: UserDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, ImageUrl")] UserDetail userDetail, IFormFile ImageUrl)
        {
            if (ModelState.IsValid && ImageUrl != null)
            {
                userDetail.ImageUrl = await BlobAzure.UploadImage(ImageUrl);
                userDetail.UserId = GetUserId();

                await _apiService.AddUserDetail(userDetail);
                //_service.AddUserDetail(userDetail);
                return RedirectToAction(nameof(Index));
            }
            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _apiService.GetById(id.Value);
            if (userDetail == null)
            {
                return NotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserDetail userDetail, IFormFile ImageUrl)
        {
            if (id != userDetail.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid && ImageUrl != null)
            {
                var imageToDelete = await _apiService.GetById(userDetail.UserId);
                BlobAzure.DeletePhoto(imageToDelete.ImageUrl);

                userDetail.ImageUrl = await BlobAzure.UploadImage(ImageUrl);

                var response = await _apiService.UpdateUserDetails(userDetail);

                if (response == false) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(userDetail);
        }

        // GET: UserDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _apiService.GetById(id.Value);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        public async Task<IActionResult> DeleteRelation(Guid UserId, Guid UserId2)
        {
            var userUser = new UserUsers(UserId, UserId2);
            var id = await _userUsersRepository.RemoveRelation(userUser);

            return RedirectToAction("Details", new { Id = id });
        }


        public async Task<IActionResult> CreateRelation(Guid? UserId)
        {
            var viewModel = new DetailsUserUserViewModel();

            viewModel.userDetail = await _apiService.GetById(UserId.Value);

            var usersIds = await GetUserCanBeAdded(UserId.Value);
            viewModel.userUsers = new List<UserUsersViewModel>();
            foreach (var item in usersIds)
            {
                var user = await _apiService.GetById(item);
                var userToAdd = new UserUsersViewModel()
                {
                    UserId = user.UserId,
                    Name = user.Name
                };
                viewModel.userUsers.Add(userToAdd);
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRelation(Guid UserId, Guid UserId2)
        {
            if (ModelState.IsValid)
            {
                var userUser = new UserUsers(UserId, UserId2);
                await _userUsersRepository.Add(userUser);
                return RedirectToAction("Details", new { id = UserId });
            }
            return View();
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid UserId)
        {
            var userDetail = await _apiService.GetById(UserId);
            if (userDetail != null)
            {
                var linkedusers = await _userUsersRepository.GetLinkedUserUsers(UserId);
                foreach (var linkeduser in linkedusers)
                {
                    await _userUsersRepository.Remove(linkeduser);
                }
                var post = await _postRepository.DeleteAllByUserId(UserId);
                var comments = await _commentRepository.DeleteAllRemainLinkedComments(UserId);
                await _apiService.DeleteUserDetails(UserId);
            }

            return RedirectToAction(nameof(Index));
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private async Task<List<Guid>> GetUserCanBeAdded(Guid id)
        {
            var allUsers = await _apiService.GetAll();
            var usersToDelete = await _userUsersRepository.GetLinkedUserUsers(id);

            var listIdsCanBeAdded = new List<Guid>();

            var listUsersCannotBeAdded = new List<Guid>();
            foreach (var item in usersToDelete)
            {
                if (item.UserId != id)
                {
                    listUsersCannotBeAdded.Add(item.UserId);
                }
                else if (item.User2Id != id)
                {
                    listUsersCannotBeAdded.Add(item.User2Id);
                }
            }
            listUsersCannotBeAdded.Add(id);

            foreach (var item in allUsers)
            {
                listIdsCanBeAdded.Add(item.UserId);
            }

            return listIdsCanBeAdded.Except(listUsersCannotBeAdded).ToList();
        }
    }
}
