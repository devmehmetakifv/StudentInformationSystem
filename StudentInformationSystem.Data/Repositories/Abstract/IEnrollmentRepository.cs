using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Data.Repositories.Abstract
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
	{
		public IEnumerable<Enrollment> GetEnrollmentsByStudentId(string studentId);
	}
}
