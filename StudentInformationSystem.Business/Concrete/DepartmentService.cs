using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Generic.Concrete;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Concrete
{
	public class DepartmentService : GenericService<Department>, IDepartmentService
	{
		private readonly IDepartmentRepository _departmentRepository;
		public DepartmentService(IDepartmentRepository repository) : base(repository)
		{
			_departmentRepository = repository;
		}
		public int GetDepartmentIdByName(string departmentName)
		{
			return _departmentRepository.GetDepartmentIdByName(departmentName);
		}
        public Department GetDepartmentByName(string departmentName)
		{
			return _departmentRepository.GetDepartmentByName(departmentName);
		}
    }
}
