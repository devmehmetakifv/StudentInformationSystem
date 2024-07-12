using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Entity.Concrete;
using StudentInformationSystem.Models;
using System.Diagnostics;

namespace StudentInformationSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseService _courseService;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, ICourseService courseService, SignInManager<User> signInManager)
        {
            _logger = logger;
            _courseService = courseService;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
