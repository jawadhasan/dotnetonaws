using System.Collections.Generic;
using demoApp.Core;
using demoApp.Data;
using demoApp.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("getUsersByName")]
        public IActionResult GetUsersByName(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var users = _userRepository.GetUsersByName(searchTerm);

                if(users != null) return Ok(users);

                 return Ok(new List<User>{new User(){Id = -1}});

            }
            return Ok(new List<User>{new User(){Id = -1}});
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetById(id);
            return Ok(user);
        }


        [HttpPost("add")]
        public IActionResult Add([FromBody] UserDto userDto)
        {

            var user = new User();
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var insertedUser = _userRepository.Insert(user);

            return Ok(insertedUser);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userRepository.RemoveById(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDto userDto)
        {

            var editableUser = _userRepository.GetById(id);

            //TODO: Validation

            editableUser.FirstName = userDto.FirstName;
            editableUser.LastName = userDto.LastName;

            var updatedUser = _userRepository.Update(editableUser);

            return Ok(updatedUser);
        }


        [HttpGet("posts")]
        public IActionResult GetPosts()
        {
            var data = _userRepository.GetPosts();
            return Ok(data);
        }

        [HttpGet("posts/{id}")]
        public IActionResult GetPostsForUser(int id)
        {
            if (id == -1)
            {
                return Ok(_userRepository.GetPosts());
            }
            var data = _userRepository.GetPostsForUser(id);
            return Ok(data);
        }
    }
}
