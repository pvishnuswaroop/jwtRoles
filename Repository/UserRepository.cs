using QuickServeAPP.DTOs;
using QuickServeAPP.Repository;
using QuickServeAPP.Data;
using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveUsersAsync()
        {
            return await _context.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserStatusAsync(User user)
        {
            var existingUser = await GetUserByIdAsync(user.UserID);
            if (existingUser == null)
                return false;

            existingUser.IsActive = user.IsActive; // Update the isActive field
            _context.Users.Update(existingUser); // Mark the user as modified
            await _context.SaveChangesAsync(); // Save the changes to the database
            return true;
        }



    }
}