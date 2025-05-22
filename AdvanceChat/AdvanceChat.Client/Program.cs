using AdvanceChat.Client.Data;
using AdvanceChat.Client.Models;
using AdvanceChat.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//-------------------------------------Added by varsha--------------------
builder.Services.AddScoped<IMyHubConnectionService, MyHubConnectionService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
builder.Services.AddScoped<ReceiverUser>();
//------------------------------------------------------------------------
await builder.Build().RunAsync();
