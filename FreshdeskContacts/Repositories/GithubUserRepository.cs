using FreshdeskContacts.Database;
using FreshdeskContacts.Models;

namespace FreshdeskContacts.Repositories
{
    public class GithubUserRepository : IGithubUserRepository
    {
        private readonly MyDbContext _context;

        public GithubUserRepository(MyDbContext context)
        {
            _context = context;
        }

        public void AddUser(GithubUser user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(int userId, GithubUser user)
        {
            var existingUser = _context.Users.Find(userId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Login = user.Login;
                existingUser.CreatedAt = user.CreatedAt;
                existingUser.HtmlUrl = user.HtmlUrl;
                existingUser.Email = user.Email;
                existingUser.Description = user.Description;
                
                _context.SaveChanges();
            }

        }

        public void DeleteUser(int userId)
        {
            var existingUser = _context.Users.Find(userId);
            _context.Users.Remove(existingUser);
            _context.SaveChanges();
        }

        public GithubUser? GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public GithubUser? GetUserByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public IEnumerable<GithubUser> GetUsers()
        {
            return _context.Users;
        }
    }
}
