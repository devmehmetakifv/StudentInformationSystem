﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Abstract
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByDepartmentIdAsync(int departmentId);
        public Course GetCourseByName(string courseName);
    }
}
