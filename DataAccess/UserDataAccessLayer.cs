using BackEnd.Interfaces;
using BackEnd.Models;
using System;
using System.Linq;

namespace BackEnd.DataAccess
{
    public class UserDataAccessLayer : IUserService
    {
        readonly BookDBContext _dbContext;

        public UserDataAccessLayer(BookDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserMaster AuthenticateUser(UserMaster loginCredentials)
        {
            UserMaster user = new UserMaster();

            var userDetails = _dbContext.UserMasters.FirstOrDefault(
                u => u.Username == loginCredentials.Username && u.Password == loginCredentials.Password
                );

            if (userDetails != null)
            {

                user = new UserMaster
                {
                    Username = userDetails.Username,
                    UserId = userDetails.UserId,
                    UserTypeId = userDetails.UserTypeId
                };
                return user;
            }
            else
            {
                return userDetails;
            }
        }

        public int RegisterUser(UserMaster userData)
        {
            try
            {
                userData.UserTypeId = 2;
                _dbContext.UserMasters.Add(userData);
                _dbContext.SaveChanges();
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public bool CheckUserAvailabity(string userName)
        {
            string user = _dbContext.UserMasters.FirstOrDefault(x => x.Username == userName)?.ToString();

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isUserExists(int userId)
        {
            string user = _dbContext.UserMasters.FirstOrDefault(x => x.UserId == userId)?.ToString();

            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
