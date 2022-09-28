using BuyU.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BuyU.Data;
using BuyUContext = BuyU.Models.BuyUContext;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BuyUContextConnection") ?? throw new InvalidOperationException("Connection string 'BuyUContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextFactory<BuyUContext>(options => options.UseSqlServer(
    "Data Source=.;Initial Catalog=BuyU;Integrated Security=true;MultipleActiveResultsets=true;"
   ),
    ServiceLifetime.Transient);
builder.Services.AddDbContextFactory<BuyUIdentityContext>(options => options.UseSqlServer(
    "Data Source=.;Initial Catalog=BuyU;Integrated Security=true;MultipleActiveResultsets=true;"
   ),
    ServiceLifetime.Transient
);



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BuyUIdentityContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<BuyUUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<BuyUContext>();
var app = builder.Build();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
