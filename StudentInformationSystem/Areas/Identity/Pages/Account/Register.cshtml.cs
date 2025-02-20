﻿#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using StudentInformationSystem.Entity.Concrete;
using StudentInformationSystem.Business.Abstract;

namespace StudentInformationSystem.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProgramService _programService;
        private readonly IDepartmentService _departmentService;

		public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IProgramService programService,
            IDepartmentService departmentService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _programService = programService;
            _departmentService = departmentService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            //[Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            //[Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
			[Required]
			//[Display(Name = "First Name")]
			public string FirstName { get; set; }
			[Required]
			//[Display(Name = "Last Name")]
			public string LastName { get; set; }
			[Required]
			//[Display(Name = "Program Name")]
			public string ProgramName { get; set; }
			[Required]
			//[Display(Name = "Contact")]
			public string Contact { get; set; }
			[Required]
			//[Display(Name = "Gender")]
			public string Gender { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("/Panel");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                string filePath = @"C:\\Users\\Mehmet Akif\\source\\repos\\StudentInformationSystem\\StudentInformationSystem\\External\\userList.json";
				string json = System.IO.File.ReadAllText(filePath);

				var userList = JsonConvert.DeserializeObject<UserList>(json);
				string emailToCheck = Input.Email;
				string role = GetRoleByEmail(userList, emailToCheck);

				if (role != null)
				{
					User user = new User();

					await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
					await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
					var result = await _userManager.CreateAsync(user, Input.Password);

					if (result.Succeeded)
					{
                        _logger.LogInformation("User created a new account with password.");
						await _userManager.AddToRoleAsync(user, role);
						
						
						var userId = await _userManager.GetUserIdAsync(user);
						var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
							protocol: Request.Scheme);

						await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
							$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

						if (_userManager.Options.SignIn.RequireConfirmedAccount)
						{
							return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
						}
						else
						{
							await _signInManager.SignInAsync(user, isPersistent: false);
                            return Redirect("/Panel");
                        }
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				else
				{
					Console.WriteLine($"{emailToCheck} is not found in any role");
				}
            }
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
		public static string GetRoleByEmail(UserList userList, string email)
		{
			foreach (var role in userList.Roles)
			{
				if (role.Value.Contains(email))
				{
					return role.Key;
				}
			}
			return null;
		}
	}
	public class UserList
	{
		public Dictionary<string, List<string>> Roles { get; set; }
	}
}
