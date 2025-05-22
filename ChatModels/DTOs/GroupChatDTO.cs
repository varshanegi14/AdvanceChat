using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModels.DTOs
{
    public class GroupChatDTO
    {
        public int ChatId { get; set; }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? SenderName { get; set; }
        [Required]
        public string? message { get; set; }
        [Required]
        public DateTime datetime { get; set; }
    }
}
