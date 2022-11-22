using BuyU.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BuyUContext = BuyU.Models.BuyUContext;
using NToastNotify;
using BuyU.Controllers;
using BuyU.Hubs;
using Microsoft.Extensions.Options;
using BuyU.Controllers.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BuyUContextConnection") ?? throw new InvalidOperationException("Connection string 'BuyUContextConnection' not found.");

// Add services to the container.
builder.Services.AddDbContext<BuyUContext>(options =>
{
    options.UseSqlServer(connectionString);
    

    });

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true,
});

builder.Services.AddSignalR();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BuyUContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//    options.RoutePrefix = string.Empty;
//});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyU");

});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();


}
app.UseHttpsRedirection();
app.UseStaticFiles();


// This middleware serves the Swagger documentation UI


app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//name: "default",
//    pattern: "{controller=Home}/{action=Index}/{pageSize}");


app.MapControllers();

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

app.Run();
