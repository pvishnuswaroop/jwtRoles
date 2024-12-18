using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllActiveUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> UpdateUserStatusAsync(User user);
    }
}