using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Concrete;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Data.Repositories.Concrete
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
	{
		public EnrollmentRepository(ApplicationDbContext context) : base(context)
		{
		}
		public IEnumerable<Enrollment> GetEnrollmentsByStudentId(string studentId)
        {
            return _context.Enrollments.Where(e => e.StudentID == studentId).ToList();
        }
	}
}
