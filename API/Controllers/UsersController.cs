using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using API.Helpers;

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
        private readonly IPhotoService _photoservice;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoservice)
        {
            _photoservice = photoservice;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        //api/users
        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            //  var users=_context.Users.ToListAsync();
            // return  await users;

            //return await _context.Users.ToListAsync();
            //return Ok(await _userRepository.GetUserAsync());

            //var users = await _userRepository.GetUserAsync();
            //map from users to memberdto
            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername=user.UserName;

            if(string.IsNullOrEmpty(userParams.Gender))
            userParams.Gender=user.Gender =="male"?"female":"male";

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);

            return Ok(users);
        }

        //api/users/1
        // [Authorize]
        [HttpGet("{username}",Name ="GetUser")]
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
            //var username=User.GetUsername();//User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); //this includes the retrieval of photos as well

            var result = await _photoservice.AddPhotoAsync(file); //upload to cloudinary 

            if(result.Error!=null) return BadRequest(result.Error.Message);

            var photo=new Photo
            {
                Url=result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };

            if(user.Photos.Count==0)
            {
                photo.IsMain=true;            
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()) 
            {
                //return _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetUser",new {username=user.UserName} ,_mapper.Map<PhotoDto>(photo)); //return 201 status code 
            }
            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId); // no need use async here since we dont get the data from db at this line of code
            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain); //this will give us the current main photo
            if(currentMain!=null) currentMain.IsMain=false; //change the previous main photo to not main 
            photo.IsMain=true; //set the new main photo 

            if(await _userRepository.SaveAllAsync()) return NoContent(); //after set the new photo as main, save it. 

            return BadRequest("Failed to set main photo");
        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if(photo==null) return NotFound();
            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId !=null) 
            {
                var result = await _photoservice.DeletePhotoAsync(photo.PublicId); //delete from cloudinary
                if(result.Error !=null) return BadRequest(result.Error.Message); //this code is for  something went wrong, not able to delete from cloudinary
            }

            user.Photos.Remove(photo); //remove the photo from the list of user's photos

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");        
        }

    }
}