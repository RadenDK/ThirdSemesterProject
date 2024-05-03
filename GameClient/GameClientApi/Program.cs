using GameClientApi.DatabaseAccessors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPlayerDatabaseAccessor, PlayerDatabaseAccessor>();
builder.Services.AddScoped<IGameLobbyDatabaseAccessor, GameLobbyDatabaseAccessor>();
builder.Services.AddScoped<IAdminDatabaseAccessor, AdminDatabaseAccessor>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseHttpsRedirection();
app.UseAuthorization();

// Use CORS middleware
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
