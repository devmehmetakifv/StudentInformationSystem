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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
	{
		public StudentRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
