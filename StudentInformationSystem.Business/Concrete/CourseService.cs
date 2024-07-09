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
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository) : base(courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetById(id);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAll();
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _courseRepository.GetCoursesByDepartmentIdAsync(departmentId);
        }

        public async Task AddCourseAsync(Course course)
        {
            await _courseRepository.Add(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.Update(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetById(id);
            if (course != null)
            {
                await _courseRepository.Delete(course);
            }
        }
    }
}
