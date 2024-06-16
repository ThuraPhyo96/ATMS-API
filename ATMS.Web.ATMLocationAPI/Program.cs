using ATMS.Web.ATMLocationAPI.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ATMS.Web.ATMLocationAPI.AppServices;

var allowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("https://localhost:7158")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure services...
builder.Services.AddDbContext<ATMLocationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Create auto mapper configuration
var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ATMS.Web.ATMLocationAPI.Mapping.MappingProfile());
});
var mapper = mapperConfiguration.CreateMapper();
// Register auto mapper to services throught lifetime use
builder.Services.AddSingleton(mapper);

// Register sevices
builder.Services.AddScoped<IATMLocationAppService, ATMLocationAppService>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ATMLocationContext>();
    try
    {
        await SeedData.Initialize(dbContext);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(allowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
