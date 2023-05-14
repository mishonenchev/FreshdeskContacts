using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshdeskContacts.Models;

namespace FreshdeskContacts.Services
{
    public interface IGithubUserService
    {
        void AddUser(GithubUser user);
        GithubUser? GetUserById(int id);
        GithubUser? GetUserByLogin(string login);
        IEnumerable<GithubUser> GetAllUsers();
        void UpdateUser(int userId, GithubUser user);
        void DeleteUser(int userId);
    }
}
