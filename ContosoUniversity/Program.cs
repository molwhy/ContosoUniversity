using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Datas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add efcore
IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json", false, false);
IConfigurationRoot configuration = configurationBuilder.Build();
builder.Services.AddDbContext<SchoolContext>(
    options => {
        options.UseSqlServer(configuration.GetConnectionString("ConnectionStrings"));
    }); 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

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
