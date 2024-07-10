using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Department
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}
