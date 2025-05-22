using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModels
{
    public class IndividualChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? ReceiverId { get; set; }
        [Required]
        public string? message { get; set; }
        public DateTime datetime { get; set; }
    }
}
