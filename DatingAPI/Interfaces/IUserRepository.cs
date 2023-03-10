using DatingAPI.DTOs;
using DatingAPI.Entities;

namespace DatingAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUsernameAsync(string username);

         Task<IEnumerable<MemberDTO>> GetMembersAsync();

        Task<MemberDTO> GetMemberByUsernameAsync(string username);
    }
}