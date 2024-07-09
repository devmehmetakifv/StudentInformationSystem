using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Course
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Credit { get; set; }
    public int DepartmentID { get; set; }
}
