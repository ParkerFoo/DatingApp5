using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<MemberDto> GetMemberAsync(string username)
        
        {
            return await _context.Users.Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //automapper to take from users table only the fields that the Dto has.
            .SingleOrDefaultAsync();
        }

       

        //public async Task<IEnumerable<MemberDto>> GetMembersAsync()
          public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
           //return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();        
        //    var query= _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking() //asnotracking used since we only need to read 
        //              .AsQueryable();

            var query = _context.Users.AsQueryable();
         
            //These 2 lines below is we filter first. If current user is male, will show female and current user wont be shown in the member lists page
            query=query.Where(u=>u.UserName!=userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob=DateTime.Today.AddYears(-userParams.MaxAge-1);
            var maxDob=DateTime.Today.AddYears(-userParams.MinAge);

            query=query.Where(u=>u.DateOfBirth>=minDob && u.DateOfBirth<=maxDob);
            
            query=userParams.OrderBy switch
            {//newer switch
                "created"=>query.OrderByDescending(u=>u.Created),
                _=>query.OrderByDescending(u=>u.LastActive) //case for default 
            };

           return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
           userParams.PageNumber, userParams.PageSize);
        }


        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //if something has been changed it will be more than zero. 
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified; //allow EF to flag that by saying yup that has been modified.
        }

      
    }
}