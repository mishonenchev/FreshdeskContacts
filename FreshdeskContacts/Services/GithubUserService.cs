using FreshdeskContacts.Models;
using FreshdeskContacts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshdeskContacts.Services
{
    public class GithubUserService : IGithubUserService
    {
        private readonly IGithubUserRepository _userRepository;

        public GithubUserService(IGithubUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(GithubUser user)
        {
            var existingUser = _userRepository.GetUserByLogin(user.Login);
            if (existingUser == null)
            {
                _userRepository.AddUser(user);
            }
            else throw new Exception("User with same login already exists");
        }

        public GithubUser? GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public GithubUser? GetUserByLogin(string login)
        {
            return _userRepository.GetUserByLogin(login);
        }

        public IEnumerable<GithubUser> GetAllUsers()
        {
            return _userRepository.GetUsers();
        }

        public void UpdateUser(int userId, GithubUser user)
        {
            var existingUser = _userRepository.GetUserById(userId);
            if (existingUser != null)
            {
                _userRepository.UpdateUser(userId, user);
            }
            else throw new Exception("User with such id not found");
        }

        public void DeleteUser(int userId)
        {
            var existingUser = _userRepository.GetUserById(userId);
            if (existingUser != null)
            {
                _userRepository.DeleteUser(userId);
            }
            else throw new Exception("User with such id not found");
        }
    }
}
