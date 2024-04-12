using Microsoft.EntityFrameworkCore;
using platformService.DataContext;
using platformService.Repositories;
using platformService.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

Console.WriteLine("--> Setting Services ...");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemory"));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

Console.WriteLine("--> Services are set successfully!");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await PreparationDb.EnsurePopulated(app);

Console.WriteLine("--> Data Is populated!");


app.UseHttpsRedirection();

Console.WriteLine("--> Building App...");

app.Run();