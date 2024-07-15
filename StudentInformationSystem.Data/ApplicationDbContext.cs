using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<Enrollment> Enrollments { get; set; }
    public virtual DbSet<Program> Programs { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<InstructorMessage> InstructorMessages { get; set; }
}
