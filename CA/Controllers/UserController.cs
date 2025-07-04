using CA.Models;
using CA.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CA.Controllers
{
    public class UserController : Controller
    {
        private UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("/User/findUserByName")]
        public User findUserByName(string username)
        {
            return _userRepository.GetUserByName(username);
        }

        [HttpGet("/User/findUserById")]
        public User findUserById(string username)
        {
            return _userRepository.GetUserById(username);
        }

        [HttpPost("/User/AddUser")]
        public String AddUser([FromBody] User user)
        {
            Console.WriteLine(user.Username);
            User newUser = new User(user.Username,user.Password);
            return _userRepository.AddUser(newUser); 
        }

        [HttpPost("/User/Login")]
        public String Login([FromBody]LoginValid loginValid)
        {
            User user = _userRepository.GetUserByName(loginValid.username);
            if (user == null)
            {
                return "User not found";
            }
            else
            {
                if (user.Password == loginValid.password)
                {
                    ISession sessionObj = HttpContext.Session;
                    sessionObj.SetString("username", loginValid.username);
                    return "Login Success";
                }
                else
                {
                    return "Wrong Password";
                }
            }
        }

        [HttpGet("/User/isLogin")]
        public String isLogin()
        {
            ISession sessionObj = HttpContext.Session;
            if (sessionObj.GetString("username") != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        [HttpGet("/User/Logout")]
        public String Logout()
        {
            ISession sessionObj = HttpContext.Session;
            if (sessionObj.GetString("username") != null)
            {
                sessionObj.Remove("username");
                return "Logout Success";
            }
            else
            {
                return "You are not logged in";
            }
        }

    }
}
