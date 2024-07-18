using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Concrete;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Concrete
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Course>> GetCoursesByDepartmentIdAsync(int departmentId)
        {
            var courses = _context.Courses.Where(c => c.DepartmentID == departmentId);
            return await courses.ToListAsync();
        }
        public Course GetCourseByName(string courseName)
        {
            return _context.Courses.FirstOrDefault(c => c.Name == courseName);
        }
    }
}
