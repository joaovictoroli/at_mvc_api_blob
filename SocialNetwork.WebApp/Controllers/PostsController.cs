using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.WebApp.Models;
using System.Security.Claims;

namespace SocialNetwork.WebApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostRepository _postsService;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IUserUsersRepository _userUsersRepository;
        public PostsController(IPostRepository postsService, IUserDetailsRepository userDetailsRepository, IUserUsersRepository userUsersRepository)
        {
            _postsService = postsService;
            _userDetailsRepository = userDetailsRepository;
            _userUsersRepository = userUsersRepository;
        }

        // GET: PostsController
        public async Task<IActionResult> Index()
        {
            //var list = await _postsService.GetAll();
            var viewModel = new IndexPostViewModel();
            viewModel.userId = GetUserId();
            //viewModel.post = list.ToList();  
            //return View(viewModel);
            //var viewModel = new IndexPostViewModel();
            viewModel.userId = GetUserId();
            viewModel.post = await GetAllPostLinkedToUserOrUserUsers(viewModel.userId);
            return View(viewModel);
        }

        // GET: PostsController/Details/5
        public async Task<IActionResult> Details(Guid? Id)
        {      
            if (Id != null)
            {
                var post = await _postsService.GetById(Id.Value);
                return View(post);
            }

            //var post = await _postsService.GetById(postId);
            return NotFound();
        }

        // GET: PostsController/Create
        public async Task<IActionResult> Create()
        {
            Post post = new Post();
            post.UserId = GetUserId();

            var user = await _userDetailsRepository.GetById(post.UserId);
            if (user == null)
            {
                return RedirectToAction("Index", "UserDetails");
            }
            return View(post);
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (post != null)
            {
                if (ModelState.IsValid)
                {
                    await _postsService.Add(post);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(post);
        }

        // GET: PostsController/Edit/5
        public async Task<IActionResult> Edit(Guid postId)
        {
            var post = await _postsService.GetById(postId);
            return View(post);
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                if (userId == post.UserId)
                {
                    await _postsService.Update(post);
                }
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public async Task<IActionResult> DeletePost(Guid PostId)
        {
            var post = await _postsService.GetById(PostId);
            await _postsService.Remove(post);
            return RedirectToAction("Index");
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        private async Task<List<Post>> GetAllPostLinkedToUserOrUserUsers(Guid id)
        {
            var AllPosts = await _postsService.GetAll();
            var linkedUsers = await _userUsersRepository.GetLinkedUserUsers(id);

            var listUserUsers = new List<Guid>();
            foreach (var user in linkedUsers)
            {
                if (user.UserId != id)
                {
                    listUserUsers.Add(user.UserId);
                }
                if (user.User2Id != id)
                {
                    listUserUsers.Add(user.User2Id);
                }
            }
            listUserUsers.Add(id);

            var listToReturn = new List<Post>();
            foreach (var post in AllPosts)
            {
                foreach (var user in listUserUsers)
                {
                    if (post.UserId == user)
                    {
                        listToReturn.Add(post);
                    }
                }

            }
            return listToReturn;
        }
    }
}
