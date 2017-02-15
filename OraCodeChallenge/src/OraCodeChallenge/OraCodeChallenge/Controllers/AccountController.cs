using JWT;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using OraCodeChallenge.Models;
using OraCodeChallenge.Models.Entities;
using OraCodeChallenge.ViewModels;
using System.Linq;
using System.Net;
using System.Text;

namespace OraCodeChallenge.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly OraContext db = new OraContext();

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public HttpResponseMessage Register(RegisterViewModel model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User already exist.");
                }

                //Create user and save to database
                var user = CreateUser(model);

                object dbUser;

                //Create token
                var token = CreateToken(user, out dbUser);

                response = Request.CreateResponse(new { dbUser, token });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
            }

            return response;
        }

        /// <summary>
        /// Create a new user and saves it to the database
        /// </summary>
        /// <param name="registerDetails"></param>
        /// <returns></returns>
        private User CreateUser(RegisterViewModel registerDetails)
        {
            var passwordSalt = CreateSalt();
            var user = new User
            {
                Salt = passwordSalt,
                Name = registerDetails.Name,
                Email = registerDetails.Email,
                PasswordHash = EncryptPassword(registerDetails.Password, passwordSalt)
            };

            db.Users.Add(user);
            db.SaveChanges();

            return user;
        }

        /// <summary>
        ///     Creates a random salt to be used for encrypting a password
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        /// <summary>
        /// Create a Jwt with user information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        private static string CreateToken(User user, out object dbUser)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                {"email", user.Email},
                {"userId", user.UserId},
                {"role", "Admin"},
                {"sub", user.UserId},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            dbUser = new { user.Email, user.UserId };
            return token;
        }

        /// <summary>
        ///     Encrypts a password using the given salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
