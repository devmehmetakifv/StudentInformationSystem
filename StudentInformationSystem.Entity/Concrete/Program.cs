using System;
using System.Collections.Generic;

namespace StudentInformationSystem.Entity.Concrete;
using System.ComponentModel.DataAnnotations;

public partial class Program
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DurationInYears { get; set; }
    public int DepartmentID { get; set; }
}
