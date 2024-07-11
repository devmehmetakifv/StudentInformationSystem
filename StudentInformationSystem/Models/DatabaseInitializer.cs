using Microsoft.AspNetCore.Identity;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Web.Models
{
	public class DatabaseInitializer
	{
		public static async Task SeedData(UserManager<User>? userManager,
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

			//var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
			//if (adminUsers.Any())
			//{
			//	Console.WriteLine("Admin user already exists.");
			//	return;
			//}
			//else
			//{
			//	var user = new IdentityUser()
			//	{
			//		UserName = "admin",
			//		Email = "admin@admin.com",
			//	};
			//	string defaultPassword = "Admin123_";

			//	var result = await userManager.CreateAsync(user, defaultPassword);
			//	if (result.Succeeded)
			//	{
			//		await userManager.AddToRoleAsync(user, "Admin");
			//		Console.WriteLine("Admin user created. Make sure to update the default password.");
			//		Console.WriteLine($"Username: {user.UserName}, Email: {user.Email}, Password: {defaultPassword}");
			//	}
			//	else
			//	{
			//		Console.WriteLine("Admin user couldn't be created.");
			//		foreach (var error in result.Errors)
			//		{
			//			Console.WriteLine(error.Description);
			//		}
			//	}
			//}
			//var students = await userManager.GetUsersInRoleAsync("Student");
			//if (students.Any())
   //         {
   //             Console.WriteLine("Student user already exists.");
   //         }
   //         else
   //         {
   //             var user = new User()
   //             {
			//		UserName = "rick.grimes@gmail.com",
   //                 FirstName = "Rick",
   //                 LastName = "Grimes",
			//		Gender = "Male",
			//		Contact = "555-555-5555, Alexandria",
			//		Email = "rick.grimes@gmail.com",
			//		RoleId = 2,
			//		DateOfBirth = new DateOnly(1970, 1, 1),
			//		ProgramID = 1
   //             };
   //             string defaultPassword = "Admin123_";

   //             var result = await userManager.CreateAsync(user, defaultPassword);
   //             if (result.Succeeded)
   //             {
   //                 await userManager.AddToRoleAsync(user, "Student");
   //                 Console.WriteLine("Student user created. Make sure to update the default password.");
   //                 Console.WriteLine($"Username: {user.UserName}, Email: {user.Email}, Password: {defaultPassword}");
   //             }
   //             else
   //             {
   //                 Console.WriteLine("Student user couldn't be created.");
   //                 foreach (var error in result.Errors)
   //                 {
   //                     Console.WriteLine(error.Description);
   //                 }
   //             }
   //         }
		}
	}
}
