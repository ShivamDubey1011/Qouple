using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        //constructor
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// this method will going to be http post will get he username and password a
        /// and the matched with database by username and check is username is present if not then its
        /// allow to save the user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserForRegisterDto userForRegisterDto)
        {
            //making username to lower
            userForRegisterDto.Username.ToLower();

            //to check username is exists
            if (await _repo.UserExists(userForRegisterDto.Username))
                ModelState.AddModelError("Username", "Username is already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            //to create
            var createUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            return Ok(createUser);  
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();

            //generate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("FQh$WjW%,G-2D>~B8M*g+gU7F");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { tokenString });


        }
    }
}
