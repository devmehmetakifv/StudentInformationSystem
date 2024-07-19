using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entity.Concrete
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        [Required]
        public string TicketSenderUserName { get; set; }
        [Required]
        public string TicketSenderRole { get; set; }
        [Required]
        public string TicketType { get; set; }
        [Required]
        public string TicketTitle { get; set; }
        [Required]
        public string TicketContent { get; set; }
        [Required]
        public DateTime TicketDate { get; set; }
        public string TicketRespondSenderUserName { get; set; }
        public string TicketRespondContent { get; set; }
        public bool isAnswered { get; set; }
        public bool isResolved { get; set; }
    }
}
