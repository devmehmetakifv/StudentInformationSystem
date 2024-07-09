using System;
using System.Collections.Generic;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Enrollment
{
    public int ID { get; set; }
    public int CourseID { get; set; }
    public int StudentID { get; set; }
    public string Grade { get; set; }
    public DateOnly EnrollmentDate { get; set; }
}
