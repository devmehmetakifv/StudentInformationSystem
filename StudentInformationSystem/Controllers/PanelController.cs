using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentInformationSystem.Web.Controllers
{
	[Authorize]
	public class PanelController : Controller
	{
		public IActionResult Index()
		{

			return View();
		}
	}
}
