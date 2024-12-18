using QuickServeAPP.Helpers;
using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Text.RegularExpressions;

namespace QuickServeAPP.Services
{
    public class UserService : IUserService
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;

        public UserService(JwtHelper jwtHelper, IUserRepository userRepository)
        {
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            //var roleAsString = user.Role.ToString(); // Convert enum to string
            // Generate JWT
            var token = _jwtHelper.GenerateToken(user.UserID, user.Role);
            return token;
        }



        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists.");
            }

            // Hash password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            //// Convert string to UserRole enum
            //if (!Enum.TryParse(registerDto.Role, out UserRole userRole))
            //{
            //    throw new Exception("Invalid user role.");
            //}

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Role = registerDto.Role,// assign enum value
                //Address = registerDto.Address
                ContactNumber = registerDto.ContactNumber
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Name = updateUserDto.Name ?? user.Name;
            // user.Address = updateUserDto.Address ?? user.Address;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            }

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<User> AuthenticateUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            return user;
        }

        public async Task<User> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Current password is incorrect.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        // Fetch all users
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllActiveUsersAsync();
            return users.Select(user => new UserDto
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            });
        }

        // Remove a user
        public async Task<bool> RemoveUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false; // Set isActive to false to "soft delete" the user
            return await _userRepository.UpdateUserStatusAsync(user);
        }
    }
}