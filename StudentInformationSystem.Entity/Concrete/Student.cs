using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentInformationSystem.Entity.Concrete;

public partial class Student
{
	[Key]
	public int ID { get; set; }
	public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
    public int ProgramID { get; set; }
}
