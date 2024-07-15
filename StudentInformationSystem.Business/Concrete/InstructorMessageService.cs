using StudentInformationSystem.Business.Generic.Concrete;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Concrete;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Services
{
    public class InstructorMessageService : GenericService<InstructorMessage>, IInstructorMessageService
    {
        private readonly IInstructorMessageRepository _instructorMessageRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IUserRepository _userRepository;

        public InstructorMessageService(IInstructorMessageRepository instructorMessageRepository, IDepartmentRepository departmentRepository, IProgramRepository programRepository, IUserRepository userRepository) : base(instructorMessageRepository)
        {
            _instructorMessageRepository = instructorMessageRepository;
            _departmentRepository = departmentRepository;
            _programRepository = programRepository;
            _userRepository = userRepository;
        }
        public IEnumerable<InstructorMessage> GetInstructorMessagesByProgramId(int programId)
        {
            var program = _programRepository.GetById(programId);
            var department = _departmentRepository.GetById(program.Result.DepartmentID);
            var instructors = _userRepository.GetInstructorsByDepartment(department.Result.ID);
            List<InstructorMessage> messages = new List<InstructorMessage>();
            foreach (var instructor in instructors)
            {
                var message = _instructorMessageRepository.GetInstructorMessageByInstructorId(instructor.Id);
                if (message != null)
                    messages.Add(message);
            }
            return messages;
        }
    }
}
