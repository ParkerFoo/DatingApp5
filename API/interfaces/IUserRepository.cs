using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;

namespace API.interfaces
{
    public interface IUserRepository
    {
        Task<MemberDto> GetMemberAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
       
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
         Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);    
    }
}