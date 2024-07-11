using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Enrollment
{
    [Key]
    public int ID { get; set; }
    [Required]
    [ForeignKey("Course")]
    public int CourseID { get; set; }
    [Required]
    [ForeignKey("User")]
    public string StudentID { get; set; }
    [Required]
    public string Grade { get; set; }
    [Required]
    public DateOnly EnrollmentDate { get; set; }
}
