using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QASite.Data
{
    public class UserRepo
    {
        private string _connectionString;

        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void SignUp(User users, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            users.PasswordHash = hash;
            using var context = new QASiteContext(_connectionString);
            context.User.Add(users);
            context.SaveChanges();
        }
        public User Login(string email, string password)
        {
            var user = GetbyEmail(email);
            if (user == null)
            {
                return null;
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user;
            }
            return null;
        }
        public User GetbyEmail(string email)
        {
            using var context = new QASiteContext(_connectionString);
            return context.User.FirstOrDefault(u => u.Email == email);
        }
        public bool IsEmailAvailable(string email)
        {
            using var context = new QASiteContext(_connectionString);
            return context.User.All(u => u.Email != email);
        }
    }
}

