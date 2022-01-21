using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Datas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add efcore
builder.Services.AddDbContext<SchoolContext>(
    options => {
        options.UseSqlServer(
            new ConfigurationManager()
                .AddJsonFile("appsettings.json", false, false)
                .Build()
                .GetConnectionString("DefaultConnection"));
    }); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

//CreateDbIfNotExists(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void CreateDbIfNotExists(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<SchoolContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}