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
	public class ProgramService : GenericService<Program>, IProgramService
	{
		private readonly IProgramRepository _programRepository;
		public ProgramService(IProgramRepository repository) : base(repository)
		{
			_programRepository = repository;
		}
		public int GetProgramIdByName(string programName){
			return _programRepository.GetProgramIdByName(programName);
		}
	}
}
