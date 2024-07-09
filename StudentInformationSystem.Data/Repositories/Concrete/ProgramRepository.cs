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
    public class ProgramRepository : GenericRepository<Program>, IProgramRepository
	{
		public ProgramRepository(ApplicationDbContext context) : base(context)
		{
		}
		public int GetProgramIdByName(string programName)
		{
			return _context.Programs.Where(p => p.Name == programName).Select(p => p.ID).FirstOrDefault();
		}
	}
}
