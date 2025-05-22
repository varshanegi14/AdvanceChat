using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChatModels
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }
        [Required]
        public string? SenderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        public string? message { get; set; } = string.Empty;
        public DateTime? datetime{get; set;} 
    }
}
