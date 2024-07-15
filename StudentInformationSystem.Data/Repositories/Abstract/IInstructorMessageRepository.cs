using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Abstract
{
    public interface IInstructorMessageRepository : IGenericRepository<InstructorMessage>
    {
        public InstructorMessage GetInstructorMessageByInstructorId(string instructorId);
    }
}
