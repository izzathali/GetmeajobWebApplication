using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Getmeajob.Data;
using Getmeajob.Interface;
using Getmeajob.Repository;
using Getmeajob.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<ICompany, CompanyRepo>();
builder.Services.AddScoped<IJob, JobRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IResume, ResumeRepo>();
builder.Services.AddScoped<IJobSeeker, JobSeekerRepo>();

builder.Services.AddScoped<IEmail, EmailSer>();

builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddToastify(config => { config.DurationInSeconds = 1000; config.Position = Position.Right; config.Gravity = Gravity.Bottom; });


var connectionString = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddDbContext<GetmeajobDbContext>(option =>
option.UseSqlServer(connectionString), ServiceLifetime.Transient);

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

app.UseNotyf();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
