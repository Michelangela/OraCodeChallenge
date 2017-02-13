using System.Collections.Generic;

namespace OraCodeChallenge.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

    }
}