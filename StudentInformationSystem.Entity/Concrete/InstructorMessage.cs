using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentInformationSystem.Entity.Concrete
{
    public class InstructorMessage
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string InstructorID { get; set; }
        public DateTime SentDate { get; set; }
    }
}
