using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TicketHive_MadCats.Server.Data;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Server.Repos.RepoInterfaces;
using TicketHive_MadCats.Server.Repos.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("UsersDatabaseString") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

// Adds out own EventTicket database context to DIC
var connectionString2 = builder.Configuration.GetConnectionString("EventTicketDatabaseString");
builder.Services.AddDbContext<EventTicketDbContext>(options =>
{
    options.UseSqlServer(connectionString2);
});

// Adds the repos for our EventTicket database to DIC
builder.Services.AddScoped<ITicketRepo, TicketRepository>();
builder.Services.AddScoped<IEventRepo, EventRepository>();


builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(); // TODO Hope this wont break shit. Added due to POST controller method issues
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Skapar admin och vanlig användare ifall de inte finns
// Den vanliga använder är per kraven från uppgiften
using(var serviceProvider = builder.Services.BuildServiceProvider())
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.Migrate();

    // Skapar Admin rollen ifall den inte finns
    if(roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult() == false)
    {
        IdentityRole adminRole = new()
        {
            Name = "Admin"
        };

        roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
    }

    // Ifall en användare som HETER admin inte finns, skapas den
    if(signInManager.UserManager.FindByNameAsync("admin").GetAwaiter().GetResult() == null)
    {
        var adminUser = new ApplicationUser()
        {
            UserName = "admin"
        };

        var createAdminResult = signInManager.UserManager.CreateAsync(adminUser, "Password1234!").GetAwaiter().GetResult();

        if(createAdminResult.Succeeded) 
        {
            signInManager.UserManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }
        else
        {
            Debug.WriteLine("Creating admin failed even though it does not exist?");
        }
    }

    // Ifall en vanlig user användare inte finns, skapa den
    if (signInManager.UserManager.FindByNameAsync("user").GetAwaiter().GetResult() == null)
    {
        var userUser = new ApplicationUser()
        {
            UserName = "user"
        };

        var createUserResult = signInManager.UserManager.CreateAsync(userUser, "Password1234!").GetAwaiter().GetResult();
    }
}

app.Run();
