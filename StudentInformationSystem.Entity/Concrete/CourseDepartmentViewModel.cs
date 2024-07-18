using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entity.Concrete
{
    public class CourseDepartmentViewModel
    {
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Department>? Departments { get; set; }
    }
}
