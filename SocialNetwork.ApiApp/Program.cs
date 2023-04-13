using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.IUsuarioRepository.cs;
using SocialNetwork.Domain.Services;
using SocialNetwork.Infra.Context;
using SocialNetwork.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SocialNetworkDbContext, SocialNetworkDbContext>();

builder.Services.AddScoped<UserDetailService, UserDetailService>();
builder.Services.AddScoped<IUserDetailsRepository, UserDetailRepository>();
builder.Services.AddScoped<IUserImagesRepository, UserImageRepository>();
builder.Services.AddScoped<UserImageService, UserImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
