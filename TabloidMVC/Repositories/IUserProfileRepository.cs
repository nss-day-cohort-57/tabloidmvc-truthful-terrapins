using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);

        List <UserProfile> GetAllUsers();

        UserProfile GetUserById(int id);

        int CreateUser(UserProfile userProfile);
    }
}