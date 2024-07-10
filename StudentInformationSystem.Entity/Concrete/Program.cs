using System;
using System.Collections.Generic;

namespace StudentInformationSystem.Entity.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Program
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int DurationInYears { get; set; }
    [Required]
    [ForeignKey("Department")]
    public int DepartmentID { get; set; }
}
