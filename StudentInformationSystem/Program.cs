using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Business.Abstract;
using StudentInformationSystem.Business.Concrete;
using StudentInformationSystem.Business.Generic.Abstract;
using StudentInformationSystem.Business.Generic.Concrete;
using StudentInformationSystem.Business.Interfaces;
using StudentInformationSystem.Business.Services;
using StudentInformationSystem.Data;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Concrete;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Concrete;
using StudentInformationSystem.Entity.Concrete;
using StudentInformationSystem.Web.Hubs;
using StudentInformationSystem.Web.Models;
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add SignalR services.
builder.Services.AddSignalR();

//Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Register repositories.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IInstructorMessageRepository, InstructorMessageRepository>();

// Register services.
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IInstructorMessageService, InstructorMessageService>();

// Register Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();
});

app.MapHub<ChatHub>("/ChatHub");

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
//    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
//    await DatabaseInitializer.SeedData(userManager, roleManager);
//}

app.Run();
