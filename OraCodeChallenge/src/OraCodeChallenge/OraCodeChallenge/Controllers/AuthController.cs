using JWT;
using OraCodeChallenge.Models;
using OraCodeChallenge.Models.Entities;
using OraCodeChallenge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace OraCodeChallenge.Controllers
{
    [Authorize]
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        private readonly OraContext db = new OraContext();

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginViewModel model)
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (existingUser == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    string ph = EncryptPassword(model.Password, existingUser.Salt);
    
                    if (ph == existingUser.PasswordHash)
                    {
                        object dbUser;
                        var token = CreateToken(existingUser, out dbUser);
                        response = Request.CreateResponse(new { dbUser, token });
                    }
                }
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            return response;
        }

        [Route("logout")]
        [HttpGet]
        public HttpResponseMessage Logout()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
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
                {"role", "Admin"  },
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
    }


}
