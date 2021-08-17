using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helpers;

namespace API.interfaces
{
    public interface IUserRepository
    {
        Task<MemberDto> GetMemberAsync(string username);
        // Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
       
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
         Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);    
    }
}