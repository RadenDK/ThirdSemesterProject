using Microsoft.AspNetCore.Authentication.Cookies;
using WebClient.BusinessLogic;
using WebClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IGameLobbyService, GameLobbyService>();

// Register Logic to the container.
builder.Services.AddScoped<ILoginLogic, LoginLogic>();
builder.Services.AddScoped<IRegistrationLogic, RegistrationLogic>();
builder.Services.AddScoped<IGameLobbyLogic, GameLobbyLogic>();
builder.Services.AddScoped<IHomePageLogic, HomePageLogic>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

string port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    // If the PORT environment variable is set, use it to configure the app's URLs
    app.Urls.Clear();
    app.Urls.Add($"http://*:{port}");
}

app.Run();