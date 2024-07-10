using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entity.Concrete
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [AllowNull]
        public DateOnly? HireDate { get; set; } //Instructor property
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [AllowNull]
        [ForeignKey("Department")]
        public int? DepartmentID { get; set; } //Instructor property
        public DateOnly DateOfBirth { get; set; }
        [AllowNull]
        [ForeignKey("Program")]
        public int? ProgramID { get; set; } //Student property
    }
}
