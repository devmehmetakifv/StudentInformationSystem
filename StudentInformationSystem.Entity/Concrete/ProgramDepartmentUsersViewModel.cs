using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entity.Concrete
{
    public class ProgramDepartmentUsersViewModel
    {
        public IEnumerable<Program>? Programs { get; set; }
        public IEnumerable<Department>? Departments { get; set; }
        public IEnumerable<User>? Users { get; set; }
    }
}
