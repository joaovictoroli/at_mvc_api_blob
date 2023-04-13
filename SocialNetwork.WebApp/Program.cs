using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.Domain.Services;
using SocialNetwork.Infra.Context;
using SocialNetwork.Infra.Repositories;
using SocialNetwork.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
//
builder.Services.AddScoped<SocialNetworkDbContext, SocialNetworkDbContext>();
//builder.Services.AddScoped<UserDetailService, UserDetailService>();
//builder.Services.AddScoped<IUserDetailsRepository, UserDetailRepository>();
builder.Services.AddScoped<ApiUserDetailService, ApiUserDetailService>();
builder.Services.AddScoped<ApiUserImageService, ApiUserImageService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserDetailsRepository, UserDetailRepository>();
builder.Services.AddScoped<IUserUsersRepository, UserUsersRepository>();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserDetails}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
