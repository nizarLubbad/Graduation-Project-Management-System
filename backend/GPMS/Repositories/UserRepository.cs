using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        //private readonly AppDbContext _context;
        //public UserRepository(AppDbContext context) => _context = context;

        //public async Task<User?> GetByEmailAsync(string email)
        //{
        //    return await _context.Users
        //        .Include(u => u.Student)
        //        .Include(u => u.Supervisor)
        //        .FirstOrDefaultAsync(u => u.Email == email);
        //}

        //public async Task<User> AddAsync(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}

        //public async Task<User?> GetByIdAsync(long id)
        //{
        //    return await _context.Users
        //        .Include(u => u.Student)
        //        .Include(u => u.Supervisor)
        //        .FirstOrDefaultAsync(u => u.UserId == id);
        //}

        //public Task<bool> ExistsByEmailAsync(string email)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<User>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<User?> GetByIdAsync(object id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<User> UpdateAsync(User entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<bool> DeleteAsync(object id)
        //{
        //    throw new NotImplementedException();
        //}
        private readonly AppDbContext _contextU;

        public UserRepository(AppDbContext context) : base(context)
        {
            _contextU = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _contextU.Users
                .Include(u => u.Student)
                .Include(u => u.Supervisor)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _contextU.Users.AnyAsync(u => u.Email == email);
        }
    }
}
