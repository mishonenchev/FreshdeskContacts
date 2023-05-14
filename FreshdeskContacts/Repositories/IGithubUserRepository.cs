using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshdeskContacts.Models;

namespace FreshdeskContacts.Repositories
{
    public interface IGithubUserRepository
    {
        void AddUser(GithubUser user);
        void UpdateUser(int userId, GithubUser user);
        void DeleteUser(int userId);
        GithubUser? GetUserById(int userId);
        GithubUser? GetUserByLogin(string login);
        IEnumerable<GithubUser> GetUsers();
    }
}
