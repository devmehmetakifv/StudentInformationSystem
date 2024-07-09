using Microsoft.AspNetCore.Identity;

namespace StudentInformationSystem.Web.Models
{
	public class DatabaseInitializer
	{
		public static async Task SeedData(UserManager<IdentityUser>? userManager,
			RoleManager<IdentityRole>? roleManager)
		{
			if (userManager == null || roleManager == null)
			{
				throw new ArgumentNullException("UserManager or RoleManager is null.");
			}

			var exists = await roleManager.RoleExistsAsync("Admin");
			if (!exists)
			{
				Console.WriteLine("Admin role isn't defined. Creating Admin role.");
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}
			exists = await roleManager.RoleExistsAsync("Student");
			if (!exists)
			{
				Console.WriteLine("Student role isn't defined. Creating Student role.");
				await roleManager.CreateAsync(new IdentityRole("Student"));
			}
			exists = await roleManager.RoleExistsAsync("Instructor");
			if (!exists)
			{
				Console.WriteLine("Instructor role isn't defined. Creating Instructor role.");
				await roleManager.CreateAsync(new IdentityRole("Instructor"));
			}

			var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
			if (adminUsers.Any())
			{
				Console.WriteLine("Admin user already exists.");
				return;
			}
			else
			{
				var user = new IdentityUser()
				{
					UserName = "admin",
					Email = "admin@admin.com",
				};
				string defaultPassword = "Admin123_";

				var result = await userManager.CreateAsync(user, defaultPassword);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Admin");
					Console.WriteLine("Admin user created. Make sure to update the default password.");
					Console.WriteLine($"Username: {user.UserName}, Email: {user.Email}, Password: {defaultPassword}");
				}
				else
				{
					Console.WriteLine("Admin user couldn't be created.");
					foreach (var error in result.Errors)
					{
						Console.WriteLine(error.Description);
					}
				}
			}
		}
	}
}
