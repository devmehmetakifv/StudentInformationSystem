using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Concrete;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Concrete
{
    public class InstructorMessageRepository : GenericRepository<InstructorMessage>, IInstructorMessageRepository
    {
        public InstructorMessageRepository(ApplicationDbContext context) : base(context)
        {
        }
        public InstructorMessage GetInstructorMessageByInstructorId(string instructorId)
        {
            return _context.InstructorMessages.FirstOrDefault(i => i.InstructorID == instructorId);
        }
    }
}
