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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
	{
		public DepartmentRepository(ApplicationDbContext context) : base(context)
		{
		}
		public int GetDepartmentIdByName(string departmentName)
		{
			return _context.Departments.Where(d => d.Name == departmentName).Select(d => d.ID).FirstOrDefault();
		}
	}
}
