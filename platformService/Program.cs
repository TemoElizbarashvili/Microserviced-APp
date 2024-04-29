using Microsoft.EntityFrameworkCore;
using platformService.AsyncDataServices.Messaging;
using platformService.DataContext;
using platformService.Repositories;
using platformService.Repositories.Contracts;
using platformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

Console.WriteLine("--> Setting Services ...");

builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemory"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

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

app.UseAuthorization();

app.MapControllers();

app.UseCors(policy =>
{
    policy.WithOrigins();
    policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

Console.WriteLine("--> Building App...");

app.Run();