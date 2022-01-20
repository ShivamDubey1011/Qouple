using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Data
{
    public class AuthRepository : IAuthRepository
    {
        //
        private readonly DataContext _dataContext;

        //Constructor
        public AuthRepository(DataContext context)
        {
            _dataContext = context;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> Login(string username, string password)
        {
           var user= await _dataContext.Users.FirstOrDefaultAsync(x=>x.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            //auth Successfull

            return user;
        }

        /// <summary>
        /// this method will verify that the given password is matching 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var  computedHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }

        }

        public async Task<User> Register(User user, string password)
        {
            //as user will goint to pass a plane text password we 
            //are creating a byte of passwordHash and password Salt
            byte[] passwordHash, passwordSalt;
            //with the help of out we will going to set value of passwordHash and passwordSalt 
            //after calling CreatePasswordHash method;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
        /// <summary>
        /// this method will going to take three parameter 
        /// and will going to generate password will the help of all three parameter
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }
    }
}
