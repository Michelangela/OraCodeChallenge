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
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private readonly OraContext db = new OraContext();

        // GET /users
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                var dbUsers = db.Users.Select(x => new { Id = x.Id, Email = x.Email }).OrderBy(x => x.Id);

                if (dbUsers == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    response = Request.CreateResponse(dbUsers);
                }
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            return response;
        }

        // GET /users/current
        [Route("current")]
        public HttpResponseMessage Current()
        {
            HttpResponseMessage response = null;

            //extract Authorization token
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            var t = headerValues.FirstOrDefault();

            if (!string.IsNullOrEmpty(t))
            {
                var secretKey = "secretKey"; //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
                try
                {
                    var payload = JWT.JsonWebToken.DecodeToObject(t, secretKey) as IDictionary<string, object>;

                    object x = null;

                    payload.TryGetValue("email", out x);
                    
                    if (!String.IsNullOrEmpty(x.ToString()) && (ModelState.IsValid))
                    {
 
                        var existingUser = db.Users.Select(a => new { Id = a.Id, Email = a.Email }).FirstOrDefault(u => u.Email == x.ToString());

                        if (existingUser == null)
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotFound);
                        }
                        else
                        {

                            response = Request.CreateResponse(existingUser);

                        }
                    }
                    else
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    return response;
                }

                catch (JWT.SignatureVerificationException)
                {
                    Console.WriteLine("Invalid token!");
                }

            }

            return response;
        }
    }
}
