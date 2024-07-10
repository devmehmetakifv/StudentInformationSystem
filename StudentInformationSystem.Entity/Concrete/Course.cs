using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Course
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Credit { get; set; }
    [Required]
    [ForeignKey("Department")]
    public int DepartmentID { get; set; }
}
