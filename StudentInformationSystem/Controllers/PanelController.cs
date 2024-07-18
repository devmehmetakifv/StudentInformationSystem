using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
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
		private readonly ITicketService _ticketService;
        private readonly IProgramService _programService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        public PanelController(
			IEnrollmentService enrollmentService,
            UserManager<User> userManager,
            IUserService userService,
            IInstructorMessageService instructorMessageService,
            ITicketService ticketService,
            IProgramService programService,
            IDepartmentService departmentService,
            IUserStore<User> userStore)
        {
            _enrollmentService = enrollmentService;
            _userManager = userManager;
            _userService = userService;
            _instructorMessageService = instructorMessageService;
            _ticketService = ticketService;
            _programService = programService;
            _departmentService = departmentService;
            _userStore = userStore;
            _emailStore = GetEmailStore();
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
		[Authorize(Roles = "Instructor")]
		public IActionResult Broadcast()
		{
			return View();
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UserManagement()
		{
            var programs = await _programService.GetAllAsync();
            var departments = await _departmentService.GetAllAsync();
            ProgramDepartmentViewModel programDepartmentViewModel = new ProgramDepartmentViewModel
            {
                Programs = programs,
                Departments = departments
            };
			return View(programDepartmentViewModel); 
		}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewStudent(string firstName,
                            string lastName,
                            string gender,
                            string contact,
                            string programName,
                            DateOnly dateOfBirth,
                            string email,
                            string phoneNumber,
                            string password,
                            string confirmPassword)
        {
            User student = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Contact = contact,
                ProgramID = _programService.GetProgramIdByName(programName),
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                RoleId = 2
            };
            await _userStore.SetUserNameAsync(student, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(student, email, CancellationToken.None);
            if(password != confirmPassword)
            {
                return NotFound("Passwords do not match!");
            }
            var result = await _userManager.CreateAsync(student, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student, "Student");
                return RedirectToAction("UserManagement");
            }
            else 
            {
                //Let's print all errors in result.Errors while returnin NotFound()
                return NotFound("User could not be created! Error: " + result.Errors.First().Description);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewInstructor(string firstName,
                            string lastName,
                            string gender,
                            string contact,
                            string departmentName,
                            DateOnly hireDate,
                            DateOnly dateOfBirth,
                            string email,
                            string phoneNumber,
                            string password,
                            string confirmPassword)
        {
            User instructor = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Contact = contact,
                DepartmentID = _departmentService.GetDepartmentIdByName(departmentName),
                HireDate = hireDate,
                Email = email,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                RoleId = 3
            };
            await _userStore.SetUserNameAsync(instructor, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(instructor, email, CancellationToken.None);
            if (password != confirmPassword)
            {
                return NotFound("Passwords do not match!");
            }
            var result = await _userManager.CreateAsync(instructor, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(instructor, "Student");
                return RedirectToAction("UserManagement");
            }
            else
            {
                //Let's print all errors in result.Errors while returnin NotFound()
                return NotFound("User could not be created! Error: " + result.Errors.First().Description);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult BringUser(string firstName, string lastName)
        {
            var user = _userService.GetUserByNames(firstName, lastName);
            if (user.FirstName.ToLower() == firstName.ToLower() && user.LastName.ToLower() == lastName.ToLower())
            {
                return Json(new { success = true, data = user });
            }
            else
            {
                return Json(new { success = false, data = "User not found!" });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetAllAsync();
            if (departments != null)
            {
                return Json(new { success = true, data = departments });
            }
            else
            {
                return Json(new { success = false, data = "There was an error fetching departments. Please contact your database administrator." });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPrograms()
        {
            var programs = await _programService.GetAllAsync();
            if (programs != null)
            {
                return Json(new { success = true, data = programs });
            }
            else
            {
                return Json(new { success = false, data = "There was an error fetching programs. Please contact your database administrator." });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string userId, string firstName, string lastName,
                                                  string gender, string contact,  
                                                  DateOnly dateOfBirth, string phoneNumber,
                                                  string email, string departmentName = null,
                                                  string programName = null,
                                                  DateOnly? hireDate = null)
        {
            var existingUser = await _userService.GetByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            existingUser.FirstName = firstName;
            existingUser.LastName = lastName;
            existingUser.Gender = gender;
            existingUser.Contact = contact;
            existingUser.DateOfBirth = dateOfBirth;
            existingUser.PhoneNumber = phoneNumber;
            existingUser.Email = email;

            if(departmentName != null)
            {
                existingUser.DepartmentID = _departmentService.GetDepartmentIdByName(departmentName);
            }
            if(programName != null)
            {
                existingUser.ProgramID = _programService.GetProgramIdByName(programName);
            }
            if(hireDate != null)
            {
                existingUser.HireDate = hireDate;
            }

            await _userService.UpdateAsync(existingUser);
            return RedirectToAction("UserManagement");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CourseManagement()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ProgramManagement()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DepartmentManagement()
        {
            return View();
        }
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SupportTickets()
		{
			var tickets = await _ticketService.GetAllAsync();
			return View(tickets);
		}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReplyToTicket(int replyTicketId, string reply, string userNameWhoReplies)
        {
            var ticket = await _ticketService.GetByIdAsync(replyTicketId);
            ticket.TicketRespondContent = reply;
            ticket.TicketRespondSenderUserName = userNameWhoReplies;
            ticket.isResolved = true;
            await _ticketService.UpdateAsync(ticket);
            
            var tickets = await _ticketService.GetAllAsync();
            return View("SupportTickets", tickets);
        }
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
