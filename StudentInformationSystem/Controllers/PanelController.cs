using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Entity.Concrete;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using StudentInformationSystem.Web.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Hangfire;

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
        private readonly ICourseService _courseService;
        private readonly IRoleService _roleService;
        public PanelController(
			IEnrollmentService enrollmentService,
            UserManager<User> userManager,
            IUserService userService,
            IInstructorMessageService instructorMessageService,
            ITicketService ticketService,
            IProgramService programService,
            IDepartmentService departmentService,
            IUserStore<User> userStore,
            ICourseService courseService,
            IRoleService roleService)
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
            _courseService = courseService;
            _roleService = roleService;
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
            var users = await _userService.GetAllAsync();
            ProgramDepartmentUsersViewModel programDepartmentViewModel = new ProgramDepartmentUsersViewModel
            {
                Programs = programs,
                Departments = departments,
                Users = users
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
                TempData["SuccessMessage"] = "Student registered successfuly!";
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
                TempData["SuccessMessage"] = "Instructor registered successfuly!";
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
            TempData["SuccessMessage"] = "User updated successfuly!";
            return RedirectToAction("UserManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string deleteUserId)
        {
            var user = await _userService.GetByIdAsync(deleteUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            await _userService.DeleteAsync(deleteUserId);

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction("UserManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseManagement()
        {
            var departments = await _departmentService.GetAllAsync();
            var courses = await _courseService.GetAllAsync();
            CourseDepartmentViewModel courseDepartmentViewModel = new CourseDepartmentViewModel
            {
                Courses = courses,
                Departments = departments
            };
            return View(courseDepartmentViewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewCourse(string name, int credits, string description, string departmentName)
        {
            Course course = new Course()
            {
                Name = name,
                Credit = credits,
                Description = description,
                DepartmentID = _departmentService.GetDepartmentIdByName(departmentName)
            };
            await _courseService.AddAsync(course);
            TempData["SuccessMessage"] = "Course added successfully!";
            return RedirectToAction("CourseManagement");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult BringCourse(string courseName)
        {
            var course = _courseService.GetCourseByName(courseName);
            if(course == null)
            {
                return Json(new { success = false, data = "Course not found!" });
            }
            else
            {
                return Json(new { success = true, data = course });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCourse(int courseId, string name, string description, int credits, string departmentName)
        {
            var existingCourse = await _courseService.GetByIdAsync(courseId);
            if (existingCourse == null)
            {
                return NotFound("User not found.");
            }
            existingCourse.Name = name;
            existingCourse.Description = description;
            existingCourse.Credit = credits;
            existingCourse.DepartmentID = _departmentService.GetDepartmentIdByName(departmentName);

            await _courseService.UpdateAsync(existingCourse);
            TempData["SuccessMessage"] = "Course updated successfuly!";
            return RedirectToAction("CourseManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int deleteCourseId)
        {
            await _courseService.DeleteAsync(deleteCourseId);
            TempData["SuccessMessage"] = "Course deleted successfully!";
            return RedirectToAction("CourseManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProgramManagement()
        {
            var departments = await _departmentService.GetAllAsync();
            var programs = await _programService.GetAllAsync();
            ProgramDepartmentUsersViewModel programDepartmentViewModel = new ProgramDepartmentUsersViewModel
            {
                Programs = programs,
                Departments = departments
            };
            return View(programDepartmentViewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewProgram(string name, int durationInYears, string description, string departmentName)
        {
            Entity.Concrete.Program program = new Entity.Concrete.Program()
            {
                Name = name,
                DurationInYears = durationInYears,
                Description = description,
                DepartmentID = _departmentService.GetDepartmentIdByName(departmentName)
            };
            await _programService.AddAsync(program);
            TempData["SuccessMessage"] = "Program added successfully!";
            return RedirectToAction("ProgramManagement");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult BringProgram(string programName)
        {
            var program = _programService.GetProgramByName(programName);
            if (program == null)
            {
                return Json(new { success = false, data = "Program not found!" });
            }
            else
            {
                return Json(new { success = true, data = program });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProgram(int programId, string name, string description, int durationInYears, string departmentName)
        {
            var existingProgram = await _programService.GetByIdAsync(programId);
            if (existingProgram == null)
            {
                return NotFound("User not found.");
            }
            existingProgram.Name = name;
            existingProgram.Description = description;
            existingProgram.DurationInYears = durationInYears;
            existingProgram.DepartmentID = _departmentService.GetDepartmentIdByName(departmentName);

            await _programService.UpdateAsync(existingProgram);
            TempData["SuccessMessage"] = "Program updated successfuly!";
            return RedirectToAction("ProgramManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProgram(int deleteProgramId)
        {
            await _programService.DeleteAsync(deleteProgramId);
            TempData["SuccessMessage"] = "Program deleted successfully!";
            return RedirectToAction("ProgramManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DepartmentManagement()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewDepartment(string name, string description)
        {
            Department department = new Department()
            {
                Name = name,
                Description = description,
            };
            await _departmentService.AddAsync(department);
            TempData["SuccessMessage"] = "Department added successfully!";
            return RedirectToAction("DepartmentManagement");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult BringDepartment(string departmentName)
        {
            var department = _departmentService.GetDepartmentByName(departmentName);
            if (department == null)
            {
                return Json(new { success = false, data = "Department not found!" });
            }
            else
            {
                return Json(new { success = true, data = department });
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDepartment(int departmentId, string name, string description)
        {
            var existingDepartment = await _departmentService.GetByIdAsync(departmentId);
            if (existingDepartment == null)
            {
                return NotFound("Department not found.");
            }
            existingDepartment.Name = name;
            existingDepartment.Description = description;

            await _departmentService.UpdateAsync(existingDepartment);
            TempData["SuccessMessage"] = "Department updated successfuly!";
            return RedirectToAction("DepartmentManagement");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int deleteDepartmentId)
        {
            await _departmentService.DeleteAsync(deleteDepartmentId);
            TempData["SuccessMessage"] = "Department deleted successfully!";
            return RedirectToAction("DepartmentManagement");
        }
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> SupportTickets()
		{
			var tickets = await _ticketService.GetAllAsync();
			return View(tickets);
		}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReplyToTicket(int replyTicketId, string reply, string userNameWhoReplies)
        {
            var ticket = await _ticketService.GetByIdAsync(replyTicketId);
            ticket.TicketRespondContent = reply;
            ticket.TicketRespondSenderUserName = userNameWhoReplies;
            ticket.isAnswered = true;
            await _ticketService.UpdateAsync(ticket);
            
            var tickets = await _ticketService.GetAllAsync();
            return View("SupportTickets", tickets);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ResolvedTicket(int ticketId)
        {
            var ticket = await _ticketService.GetByIdAsync(ticketId);
            ticket.isResolved = true;
            await _ticketService.UpdateAsync(ticket);
            BackgroundJob.Schedule(() => DeleteTicket(ticketId), TimeSpan.FromMinutes(1));
            TempData["SuccessMessage"] = "Ticket marked as Resolved and will be deleted shortly. We're glad we could help.";
            return Redirect("~/Identity/Account/Manage/Support");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NotResolvedTicket(int ticketId)
        {
            var ticket = await _ticketService.GetByIdAsync(ticketId);
            ticket.isResolved = false;
            await _ticketService.UpdateAsync(ticket);
            return RedirectToAction("RedirectToSendEmail", new { ticketId = ticket.TicketId });
        }
        [Authorize]
        public IActionResult RedirectToSendEmail(int ticketId)
        {
            var encryptedTicketId = ChecksumValidation.Encrypt(ticketId.ToString());
            return RedirectToAction("SendEmail", new { ticketId = ticketId, key = encryptedTicketId });
        }
        [Authorize]
        public async Task<IActionResult> SendEmail(int ticketId, string key)
        {
            var decryptedTicketId = ChecksumValidation.Decrypt(key);
            if (ticketId.ToString() != decryptedTicketId)
            {
                return Unauthorized();
            }
            else
            {
                var ticket = await _ticketService.GetByIdAsync(ticketId);
                return View(ticket);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailToSupport(string senderEmail, string recipientEmail, string title, string body, int ticketId)
        {
            var senderUser = _userService.GetUserByEmail(senderEmail);
            var recipientUser = _userService.GetUserByEmail(recipientEmail);
            var role = await _roleService.GetByIdAsync(recipientUser.RoleId);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderUser.FirstName + "" + senderUser.LastName, senderEmail));
            message.To.Add(new MailboxAddress(role.Name, recipientEmail));
            message.Subject = title;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("devmehmetakifv@gmail.com", "afha ktev oass nvwq");
                    client.Send(message);
                    client.Disconnect(true);

                    var ticket = await _ticketService.GetByIdAsync(ticketId);
                    ticket.isResolved = false;
                    ticket.UserResponse = "User sent email to support.";
                    await _ticketService.UpdateAsync(ticket);
                    BackgroundJob.Schedule(() => DeleteTicket(ticketId), TimeSpan.FromMinutes(1));

                    TempData["SuccessMessage"] = "Mail has been sent successfully. Thanks for reaching us. Our team will review your mail and get back to you as soon as possible.";
                }
                catch (Exception e)
                {
                    client.Disconnect(true);
                    TempData["FailedMessage"] = "Mail couldn't be sent at the moment. Please try again later.";
                }
                return Redirect("~/Identity/Account/Manage/Support");
            }
        }
        public async Task DeleteTicket(int ticketId)
        {
            await _ticketService.DeleteAsync(ticketId);
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
