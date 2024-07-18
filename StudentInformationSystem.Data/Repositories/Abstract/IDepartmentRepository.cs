using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Data.Repositories.Abstract
{
    public interface IDepartmentRepository : IGenericRepository<Department>
	{
		public int GetDepartmentIdByName(string departmentName);
        public Department GetDepartmentByName(string departmentName);
    }
}
