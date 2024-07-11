using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Web.Controllers
{
	[Authorize]
	public class PanelController : Controller
	{
		private readonly IEnrollmentService _enrollmentService;
		private readonly UserManager<User> _userManager;
        public PanelController(IEnrollmentService enrollmentService, UserManager<User> userManager)
        {
            _enrollmentService = enrollmentService;
			_userManager = userManager;
        }
        public IActionResult Index()
		{
			return View();
		}
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> Enrollments()
		{
			var user = await _userManager.GetUserAsync(User);
			if(user == null)
            {
                return NotFound();
            }
			var enrollments = _enrollmentService.GetEnrollmentsByStudentId(user.Id);
			return View(enrollments);
		}
	}
}
