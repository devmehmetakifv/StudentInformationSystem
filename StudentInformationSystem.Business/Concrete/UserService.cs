using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Generic.Concrete;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IProgramRepository _programRepository;

        public UserService(IUserRepository userRepository, IDepartmentRepository departmentRepository, IProgramRepository programRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _programRepository = programRepository;
        }
        public string GetUserEmailAddress(User user)
        {
            return _userRepository.GetUserEmailAddress(user);
        }
        public string GetUserRoleByUserName(string userName)
        {
            return _userRepository.GetUserRoleByUserName(userName);
        }
        public IEnumerable<User> GetStudentsByInstructor(int departmendId)
        {
            var department = _departmentRepository.GetById(departmendId);
            var programs = _programRepository.GetProgramsWithDepartmendId(departmendId);
            List<User> students = new List<User>();
            foreach (var program in programs)
            {
                var student = _userRepository.GetStudentByProgramId(program.ID);
                if(student != null)
                    students.Add(student);
            }
            return students;
        }
        public IEnumerable<User> GetInstructorsByDepartment(int departmentId)
        {
            return _userRepository.GetInstructorsByDepartment(departmentId);
        }
    }
}
