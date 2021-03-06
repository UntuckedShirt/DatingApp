using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

// this controller will handle tokens
namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        // constructor
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        // allows method creation
        [HttpPost("register")]
        // method below is register
        // Appuser is what is being called back
        // this binds to BaseApiController
        // another attrubyrte needs to be added to the 
        // parameters using From. We are using the 
        // ApiController atribute its no necessary
        // the api controller is smdart enough to figure 
        // out where the info is coming from

        // you need to replace what user is returning. You need
        // to replace what was once <AppUser> and turn it the 
        // <UserDto>. DTO meaning (Data Transfer Object)
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // this using statement of HMACHA512 below helps 
            // with the hashing by providing and algorithm
            // using ensures that when the class below is 
            // finished with the class ofHMACSHA512 it will be // disposed of corretly
            // everytime we use a class inside of a using 
            // statment it call the dispose method
            // any class using the dispose mnethod creates 
            // something called an Idisposalbe interface. 

            // we get access to the list property or 
            // obj because we use an ActionResult
            // this is where we check to see matching username
            if (await UserExists(registerDto.UserName)) return BadRequest("username is taken");

            using var hmac = new HMACSHA512();
            // hmac allows the salting of keys
            var user = new AppUser
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            // what below is doing is that it wants to add
            // user to our users table
            // look up what Add does specifically
            // all this allows the Register method
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // user needs to be converted to UserDto. user is 
            // returning AppUser which we changed. We must make 
            // it a new UserDto

            return new UserDto
            {
                username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        // this will be where we create a login
        [HttpPost("login")]

        // update all return types to <UserDto>>

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // this gets user from the database
            // SingleOrDefault returns the only element of a 
            // sequence or default value if a sequence is empty
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid username");

            // if we find user we need to use hmac
            // the password salt goes within the parentheses
            // it gives them the same computed hash
            using var hmac = new HMACSHA512(user.PasswordSalt);
            // we need to work out the hash for the apssword 
            // loginDto
            // below compares identical passwords to allow user 
            // login
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

             return new UserDto
            {
                username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}