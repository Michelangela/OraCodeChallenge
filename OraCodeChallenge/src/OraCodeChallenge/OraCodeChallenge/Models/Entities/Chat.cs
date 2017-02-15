using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OraCodeChallenge.Models.Entities
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public string Name { get; set; }

        public virtual List<ChatMessage> ChatMessages { get; set; }
    }
}