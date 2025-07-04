using CA.Models;
using CA.Services;

namespace CA.Repository
{
    public class UserRepository
    {
        public MyDbContext _context;
        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public User GetUserById(String userId)
        {
            User? user = _context.User.FirstOrDefault(u=>u.Id == userId);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public User GetUserByName(string userName)
        {
            User? user = _context.User.FirstOrDefault(u => u.Username == userName);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public String AddUser(User user)
        {
            if (_context.User.FirstOrDefault(user1 => user1.Id == user.Id) == null)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return "Add Success";
            }
            else
            {
                return "User already Exist";
            }
        }

    }
}
