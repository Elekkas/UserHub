using Authorization;
using Authorization.Interfaces;
using Blazored.LocalStorage;
using EFDataAccessLibrary.DataAccess;
using Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMvcCore(options => options.EnableEndpointRouting = false);
builder.Services.AddDbContext<WebDatabaseContext>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ICryptography, Cryptography>();
builder.Services.AddScoped<IUserManagement, UserManagement>();
builder.Services.AddScoped<IAccessToken, AccessToken>();
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);
builder.Services.AddResponseCompression(options => {
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseMvcWithDefaultRoute();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chatroom");
app.MapFallbackToPage("/_Host");

app.Run();
