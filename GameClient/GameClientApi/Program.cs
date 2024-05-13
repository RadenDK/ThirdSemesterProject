using GameClientApi.DatabaseAccessors;
using GameClientApi.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPlayerDatabaseAccessor, PlayerDatabaseAccessor>();
builder.Services.AddScoped<IGameLobbyDatabaseAccessor, GameLobbyDatabaseAccessor>();
builder.Services.AddScoped<ISecurityHelper, SecurityHelper>();
builder.Services.AddScoped<IAdminDatabaseAccessor, AdminDatabaseAccessor>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Get the secret key from configuration
byte[] secretKey = Encoding.UTF8.GetBytes(builder.Configuration["SECRET_KEY"]);

// Configure the JWT Authentication Service
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(secretKey),
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true
	};
});


// Add CORS services
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

string port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    // If the PORT environment variable is set, use it to configure the app's URLs
    builder.WebHost.UseUrls($"http://*:{port}");
}
else
{
    // If the PORT environment variable is not set, use the default URLs
    builder.WebHost.UseUrls("http://localhost:5198;https://localhost:7092");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS middleware
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
