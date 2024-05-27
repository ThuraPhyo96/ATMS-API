using ATMS.Web.BankMvc;
using ATMS.Web.BankMvc.Data;
using ATMS.Web.BankMvc.Hubs;
using ATMS.Web.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

//string logBaseFolderPath = AppDomain.CurrentDomain.BaseDirectory;
//string logFolderPath = Path.Combine(logBaseFolderPath, "logs/ATM.Web.BankMvc_.txt");

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection")!;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true })
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();
    // Register DB Context as Transient because of middle ware usage and by default, it is scoped lifetime
    //builder.Services.AddDbContext<ApplicationDBContext>(options =>
    //{
    //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //},
    //optionsLifetime: ServiceLifetime.Transient,
    //contextLifetime: ServiceLifetime.Transient);

    builder.Services.RegisterDBContext(builder.Configuration.GetConnectionString("DefaultConnection")!);

    builder.Services.AddScoped(n => new AdoDotNetService(builder.Configuration.GetConnectionString("DefaultConnection")!));
    builder.Services.AddScoped(n => new DapperService(builder.Configuration.GetConnectionString("DefaultConnection")!));

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddSignalR();

    var app = builder.Build();

    // Seed data
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDBContext>();
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
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Index}/{id?}");

    app.MapHub<ChatHub>("/chatHub");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}