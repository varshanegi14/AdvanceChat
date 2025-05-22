using AdvanceChat.ChatHubs;
using AdvanceChat.Client.Data;
using AdvanceChat.Client.Models;
using AdvanceChat.Client.Pages;
using AdvanceChat.Client.Services;
using AdvanceChat.Components;
using AdvanceChat.Data;
using AdvanceChat.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSignalR();
//=============================================================================================
builder.Services.AddScoped<IMyHubConnectionService, MyHubConnectionService>();
builder.Services.AddScoped<IChatRepository,ChatRepository>();
builder.Services.AddScoped<ReceiverUser>();
builder.Services.AddControllers();

// 1.  Add DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 41)); // find version from mysql cmd using command -SELECT VERSION();
    options.UseMySql(connectionString, serverVersion);
});

//  2. ADD IDENTITY
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    //.AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>()  --- no need of UserClaimPrincipalFactory here, though we are generating JWT Token, UserClaimPrincipalFactory works with cookie based authentication
    .AddDefaultTokenProviders();

//  5. CREATE BELOW DEFINED ROLES IN DB
async Task CreateInitialRolesAndAdmin(IServiceProvider serviceProvider)
{
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await CreateRole(RoleManager, "SuperAdmin");
    await CreateRole(RoleManager, "Admin");
    await CreateRole(RoleManager, "Owner");
    await CreateRole(RoleManager, "User");
    //  await ChangeUserRole(UserManager, "", "SuperAdmin");
}

async Task CreateRole(RoleManager<IdentityRole> RoleManager, string role)
{
   
    IdentityResult roleResult;
    var roleCheck = await RoleManager.RoleExistsAsync(role);
    if (!roleCheck)
    {
        //create the roles and seed them to the database
        roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
    }
}

// this add when we add PersistingServerAuthenticationStateProvider class
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//===========================================================================================================================
var app = builder.Build();

//  6. ✅ Create roles after building the app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateInitialRolesAndAdmin(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AdvanceChat.Client._Imports).Assembly);

app.MapControllers();
app.MapHub<Chathub>("/chathub");

//--------Adding logout endpoint from class
app.endpointConventionBuilder();
//========================================
app.Run();
