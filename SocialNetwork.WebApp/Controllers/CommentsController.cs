using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.Domain.Services;
using SocialNetwork.Infra.Repositories;
using SocialNetwork.WebApp.Models;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SocialNetwork.WebApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {

        private readonly IPostRepository _postsService;
        private readonly ICommentRepository _commentService;
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IUserUsersRepository _userUsersRepository;

        public CommentsController(IPostRepository postsService, ICommentRepository commentService, 
            IUserDetailsRepository userDetailsRepository, IUserUsersRepository userUsersRepository)
        {
            _postsService = postsService;
            _commentService = commentService;
            _userDetailsRepository = userDetailsRepository;
            _userUsersRepository = userUsersRepository;
        }
        public async Task<IActionResult> Details(Guid? Id)
        {
            if (Id != null)
            {
                DetailsPostWCommentsViewModel detailsPost = new DetailsPostWCommentsViewModel();
                detailsPost.post = await _postsService.GetById(Id.Value);
                detailsPost.Comment = await _commentService.GetAllLinkedComment(detailsPost.post.PostId);
                detailsPost.UserId = GetUserId();
                detailsPost.HasLink = await _userUsersRepository.IsLinkedUser(detailsPost.UserId, detailsPost.post.UserId);

                return View(detailsPost);
            }
            return NotFound();
        }

        public async Task<IActionResult> Create(Guid? id)
        {
            Comment comment = new Comment();
            var post = await _postsService.GetById(id.Value);
            comment.PostId = post.PostId;
            comment.UserId = GetUserId();

            var user = await _userDetailsRepository.GetById(post.UserId);
            if (user == null)
            {
                return RedirectToAction("Index", "UserDetails");
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (comment != null)
            {
                if (ModelState.IsValid)
                {
                    await _commentService.Add(comment);
                    return RedirectToAction("Details", new { id = comment.PostId });
                }
            }
            return View(comment);
        }

        public async Task<IActionResult> DeleteComment(Guid CommentId, Guid PostId)
        {
            var comment = await _commentService.GetById(CommentId);
            await _commentService.Remove(comment);
            return RedirectToAction("Details", new { id = PostId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await _commentService.GetById(id);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentService.Update(comment);
                return RedirectToAction("Details", new { id = comment.PostId });
            }
            return View(comment);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}
