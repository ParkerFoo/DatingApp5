using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost("register")]
        //public async Task<ActionResult<AppUser>> Register(string password, string username) // you might wonder why by just putting in username and password as parameter, it will know it comes from where. Basically, this is due to [APIController] in the BaseAPIController.

        // [APIController] is smart enough to assign the requested obj into RegisterDto


        //public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) //we dont want to return AppUser anymore
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");


            using var hmac = new HMACSHA512(); //initalize the algo for HHAMCSHA512

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //ComputeHash receives byte array as parameter and return byte array. It basically creates the hash. Encoding UTF8 getbytes will turn string into bytes array.  
                PasswordSalt = hmac.Key
            };            

            _context.Users.Add(user); //the add() you dont actually add it into db yet, it only tracks it in EF
            await _context.SaveChangesAsync(); //this is when its truly saving into DB.            

            //return user;

            return new UserDto
            {
                Username= user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        //public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //find the username in the DB based on the username passed through the request
            var user = await _context.Users
            .Include(p=>p.Photos) //if don't include this, line 89 will have problem 
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            //craete HMAC algo with the key of the user. Usr password salt needs to be passed or else it will generate random hmac algo with random key. By passing salt of the user, now we can compare if the password in the request is same or not with the password in the Db.
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //create hash based on the request password
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //compare if their hash are the same or not. 
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            // return user; 
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url //if no main, return null due to optional chaining
            };
        }



        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}