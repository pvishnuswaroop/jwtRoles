using QuickServeAPP.DTOs;
using System.Threading.Tasks;
using QuickServeAPP.Models;

namespace QuickServeAPP.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(string email, string password);
        Task<User> RegisterUserAsync(RegisterDto registerDto);
        Task<User> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
        Task<User> AuthenticateUserAsync(LoginDto loginDto);
        Task<User> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(); // Fetch all users
        Task<bool> RemoveUserAsync(int userId); // Remove a user by ID
    }
}