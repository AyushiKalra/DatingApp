//ENTRY POINT FPR OUR APPLICATION
using DatingAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//adding our datacontext class as a service in our container.
builder.Services.AddDbContext<DataContext>(opt => 
{
opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});//this connection will be set in appsettings.json
//unlike SQL, we don't need server name, port number or security info in SQL Lite connection string

var app = builder.Build();

// Configure the HTTP request pipeline. This is MIDDLEWARE wherein the request comes in and we can do some things 
//such as authentication before sending a response.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();// tells the request which API endpoint controller it needs to go to.

app.Run();
