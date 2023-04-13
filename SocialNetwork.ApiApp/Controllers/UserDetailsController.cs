using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialNetwork.ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : Controller
    {
        private readonly UserDetailService _service;
        public UserDetailsController(UserDetailService service)
        {
            _service = service;
        }
        // GET: api/<UserDetailsController>
        [HttpGet]
        public async Task<IEnumerable<UserDetail>> GetAll()
        {
            return await _service.GetAll();
        }

        // GET api/<UserDetailsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetail>> GetById(Guid id)
        {
            return await _service.GetById(id);
        }

        //// POST api/<UserDetailsController>
        [HttpPost]
        public async Task<ActionResult<UserDetail>> PostUserDetail(UserDetail userDetail)
        {
            await _service.AddUserDetail(userDetail);
            return userDetail;
        }


        //// PUT api/<UserDetailsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetail>> PutUserDetail(UserDetail updatedUserDetail)
        {
            var existingUserDetail = await _service.GetById(updatedUserDetail.UserId);

            if (existingUserDetail == null)
            {
                return NotFound();            
            }

            await _service.UpdateUserDetail(updatedUserDetail);                 

            return updatedUserDetail;
        }

        //// DELETE api/<UserDetailsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDetail>> DeleteUserImage(Guid id)
        {
            var userDetail = await _service.GetById(id);
            await _service.RemoveUserDetail(userDetail);

            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }
    }
}
