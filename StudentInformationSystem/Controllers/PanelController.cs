using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Web.Controllers
{
	[Authorize]
	public class PanelController : Controller
	{
		private readonly IEnrollmentService _enrollmentService;
		private readonly IUserService _userService;
		private readonly UserManager<User> _userManager;
		private readonly IInstructorMessageService _instructorMessageService;
        public PanelController(IEnrollmentService enrollmentService, UserManager<User> userManager, IUserService userService, IInstructorMessageService instructorMessageService)
        {
            _enrollmentService = enrollmentService;
			_userManager = userManager;
			_userService = userService;
			_instructorMessageService = instructorMessageService;
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
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> Students()
		{
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.DepartmentID.HasValue)
            {
                return NotFound();
            }
            var students = _userService.GetStudentsByInstructor(user.DepartmentID.Value);
            return View(students);
        }
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> NewAnnouncement(string id)
        {
			return View();
        }
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> PostNewAnnouncement(string announcementTitle, string announcementContent, string senderEmail)
		{
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
			InstructorMessage instructorMessage = new InstructorMessage
			{
				Title = announcementTitle,
				Content = announcementContent,
				InstructorID = user.Id,
				SentDate = DateTime.Now
			};
			await _instructorMessageService.AddAsync(instructorMessage);

            TempData["SuccessMessage"] = "Announcement posted successfully.";

            return RedirectToAction("NewAnnouncement");
		}
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> Announcements()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
			var instructorMessages = _instructorMessageService.GetInstructorMessagesByProgramId(user.ProgramID.Value);
            return View(instructorMessages);
        }
	}
}
