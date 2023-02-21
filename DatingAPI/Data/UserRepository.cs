using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class UserRepository : IUserRepository
    {
        public DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context , IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(p=> p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            //eager loading : tell EF to include the related tables as well
            return await _context.Users
            .Include(p=> p.Photos).
            ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //to return bool:if something is saved into db , return true ; else false.
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            //this tells the entity framework tracker that something has changed with the entity (user).
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _context.Users
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();           
        }

        public async Task<MemberDTO> GetMemberByUsernameAsync(string username)
        {
             return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }
    }
}