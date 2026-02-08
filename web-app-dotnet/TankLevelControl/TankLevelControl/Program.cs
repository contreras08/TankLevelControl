using Microsoft.EntityFrameworkCore;
using TankLevelControl.Data;
using TankLevelControl.Hubs;

var builder = WebApplication.CreateBuilder(args);

// MVC + Controllers API
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// ✅ EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ SignalR
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ✅ API
app.MapControllers();

// ✅ SignalR Hub
app.MapHub<TankHub>("/tankHub");

// ✅ MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
