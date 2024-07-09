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
	public class EnrollmentService : GenericService<Enrollment>, IEnrollmentService
	{
		private readonly IEnrollmentRepository _enrollmentRepository;
		public EnrollmentService(IEnrollmentRepository repository) : base(repository)
		{
			_enrollmentRepository = repository;
		}
	}
}
