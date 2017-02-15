using System;
using System.ComponentModel.DataAnnotations;

namespace OraCodeChallenge.Models.Entities
{
    public class ChatMessage
    {
        [Key]
        public int ChatMessageId { get; set; }

        public int ChatId { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Text { get; set; }

        public virtual Chat Chat { get; set; }

    }
}