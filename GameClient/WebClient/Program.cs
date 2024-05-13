using Microsoft.AspNetCore.Authentication.Cookies;
using WebClient.BusinessLogic;
using WebClient.Security;
using WebClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IGameLobbyService, GameLobbyService>();
builder.Services.AddScoped<ITokenService, TokenService>();


// Register Logic to the container.
builder.Services.AddScoped<ILoginLogic, LoginLogic>();
builder.Services.AddScoped<IRegistrationLogic, RegistrationLogic>();
builder.Services.AddScoped<IGameLobbyLogic, GameLobbyLogic>();

builder.Services.AddScoped<ITokenManager, TokenManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

// Add CORS policy
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
		});
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

app.UseCors("AllowAllOrigins");

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
else
{
	// If the PORT environment variable is not set, use the default URLs
	builder.WebHost.UseUrls("http://localhost:5028;https://localhost:7292");
}

app.Run();