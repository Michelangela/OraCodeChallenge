using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.OData.Query;

namespace OraCodeChallenge.Models.Entities
{
    [Page(MaxTop = 100)]
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        public string Name { get; set; }

        public virtual List<ChatMessage> ChatMessages { get; set; }
    }
}