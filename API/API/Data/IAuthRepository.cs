using API.Models;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IAuthRepository
    {
        /// <summary>
        /// this method will going to take user and text password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>User Object </returns>
        Task<User> Register(User user, string password);

        /// <summary>
        /// this method will allow user to login by pasing username and password 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User Object </returns>
        Task<User> Login(string username, string password);

        /// <summary>
        /// this method will going to take username and going to return bool true or false based
        /// on the the username is present or not 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> UserExists(string username);
    }
}
