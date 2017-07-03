using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class UserManager
    {
        private string _connectionString;

        public UserManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(string firstName, string lastName, string emailAddress, string password)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            using (var context = new TasksDBDataContext(_connectionString))
            {
                User user = new User
                {
                    Email = emailAddress,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = hash,
                    Salt = salt
                };
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }
        }

        public User Login(string emailAddress, string password)
        {
            User user = GetUser(emailAddress);
            if (user == null)
            {
                return null;
            }

            bool isMatch = PasswordHelper.IsMatch(password, user.PasswordHash, user.Salt);
            if (isMatch)
            {
                return user;
            }
            return null;
        }

        public User GetUser(string emailAddress)
        {
            using (var context = new TasksDBDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == emailAddress);
            }
        }
    }
}