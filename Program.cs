using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using RingoMediaReminder.Data;
using RingoMediaReminder.Helpers;
using RingoMediaReminder.Services;


var builder = WebApplication.CreateBuilder(args);

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddHangfireServer();




// Add services to the container.
builder.Services.AddControllersWithViews();

// Add configuration for mail settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Add mailing service as a transient service
builder.Services.AddScoped<IMailingServices, MailingServices>();

// Add DbContext using the connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    Authorization = new[] { new Hang() } // Customize authorization as needed
//});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Departments}/{action=Index}/{id?}");

app.UseHangfireDashboard();
app.UseHangfireServer();

app.Run();
