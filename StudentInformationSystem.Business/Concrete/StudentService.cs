using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Generic.Concrete;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Concrete
{
	public class StudentService : GenericService<Student>, IStudentService
	{
		private readonly IStudentRepository _studentRepository;
		public StudentService(IStudentRepository repository) : base(repository)
		{
			_studentRepository = repository;
		}
	}
}
