using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseAPIController //inherit from BaseAPIController
    {
        //commented by FRS. Replaced by UserRepository
        // private readonly DataContext _context;

        // public UsersController(DataContext context)
        // {
        //     _context=context;            
        // }  
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        //api/users
        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //  var users=_context.Users.ToListAsync();
            // return  await users;

            //return await _context.Users.ToListAsync();
            //return Ok(await _userRepository.GetUserAsync());

            //var users = await _userRepository.GetUserAsync();
            //map from users to memberdto
            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            var usersToReturn =await _userRepository.GetMembersAsync();
            return Ok(usersToReturn);
        }

        //api/users/1
        // [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user = _context.Users.FindAsync(id);
            // return await user;

            //return await _context.Users.FindAsync(id);
            //return await _userRepository.GetUserByUsernameAsync(username);

           //var user= await _userRepository.GetUserByUsernameAsync(username);
           //map from user to memberdto
           //return _mapper.Map<MemberDto>(user);

           return await _userRepository.GetMemberAsync(username);
          
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user=await _userRepository.GetUserByUsernameAsync(username);    

            _mapper.Map(memberUpdateDto,user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }       
    }
}