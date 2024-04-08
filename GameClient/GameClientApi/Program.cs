using GameClientApi.DatabaseAccessors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// The below line makes it so that when the project is build asp.net know to create 
// a new PlayerDatabaseAccessor and give it as an constructor parameter to the controller
// if it takes an parameter like IPlayerDatabaseAccessor
builder.Services.AddScoped<IPlayerDatabaseAccessor, PlayerDatabaseAccessor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
