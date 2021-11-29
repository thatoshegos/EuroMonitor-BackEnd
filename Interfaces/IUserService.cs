using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IUserService
    {
        UserMaster AuthenticateUser(UserMaster loginCredentials);
        int RegisterUser(UserMaster userData);
        bool CheckUserAvailabity(string userName);

        bool isUserExists(int userId);
    }
}
