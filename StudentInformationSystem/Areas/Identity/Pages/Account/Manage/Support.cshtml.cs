using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using StudentInformationSystem.Entity.Concrete;
using StudentInformationSystem.Data;
using StudentInformationSystem.Business.Interfaces;

namespace AcademicianPlatform.Areas.Identity.Pages.Account.Manage
{
    public class SupportModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;

        public SupportModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            ITicketService ticketService,
            IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _ticketService = ticketService;
            _userService = userService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        //public List<Ticket> Tickets { get; set; }
        public class InputModel
        {
            [Display(Name = "Category")]
            public string? TicketType { get; set; }
            [Display(Name = "Title")]
            public string? TicketTitle { get; set; }
            [Display(Name = "Content")]
            public string? TicketContent { get; set; }
        }
        public async Task<IActionResult> OnPostSubmitTicketAsync(string userName)
        {
            Ticket ticket = new()
            {
                TicketType = Input.TicketType,
                TicketTitle = Input.TicketTitle,
                TicketContent = Input.TicketContent,
                TicketSenderRole = _userService.GetUserRoleByUserName(userName),
                TicketDate = DateTime.Now,
                TicketSenderUserName = userName,
                TicketRespondContent = " ",
                TicketRespondSenderUserName = " ",
                isResolved = false
            };
            await _ticketService.AddAsync(ticket);

            return RedirectToPage("Support");
        }
        public async Task<IActionResult> OnPostDeleteTicketAsync(int ticketId)
        {
            var ticketToDelete = await _ticketService.GetByIdAsync(ticketId);

            if (ticketToDelete == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (ticketToDelete.TicketSenderUserName != user.UserName)
            {
                return Unauthorized();
            }

            await _ticketService.DeleteAsync(ticketToDelete.TicketId);

            return RedirectToPage("Support");
        }
    }
}
