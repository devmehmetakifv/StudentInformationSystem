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
    public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
	{
		public InstructorRepository(ApplicationDbContext context) : base(context)
		{
		}
		public Instructor GetInstructorByEmail(string email)
        {
			return _context.Instructors.FirstOrDefault(i => i.Email == email) ?? throw new ArgumentNullException();
        }
	}
}
