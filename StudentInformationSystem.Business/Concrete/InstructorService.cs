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
	public class InstructorService : GenericService<Instructor>, IInstructorService
	{
		private readonly IInstructorRepository _instructorRepository;
		public InstructorService(IInstructorRepository repository) : base(repository)
		{
			_instructorRepository = repository;
		}
		public Instructor GetInstructorByEmail(string email)
        {
            return _instructorRepository.GetInstructorByEmail(email);
        }
	}
}
