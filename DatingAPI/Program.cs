using DatingAPI.Data;
using DatingAPI.Extensions;
using DatingAPI.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//adding the extension method
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline. This is MIDDLEWARE wherein the request from browser comes in and we can do some things.
//such as authentication before sending a response.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//inserting our exception handler middleware
app.UseMiddleware<ExceptionMiddleware>();

//using the CORS in middleware //adding in request header that the API requesting data is safe.
app.UseCors(policyBuilder=> policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

//in order to use the authentication service, we need to add middleware to authenticate the request. ORDER IS IMPORTANT HERE.
app.UseAuthentication(); //asks DO YOU HAVE A VAID TOKEN?
app.UseAuthorization();//ok you have a valid token, what are you allowed to do?

app.MapControllers();// tells the request which API endpoint controller it needs to go to.

//adding seed data to our EF migration.
using var scope= app.Services.CreateScope();//gives access to the services inside the program class.
var services = scope.ServiceProvider;
//since it is not a HTTP request:
try{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
//Asynchronously applies any pending migrations for the context to the database. Will create the database if it does not already exist.
    await Seed.SeedUsers(context);

}catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex , "An error has occured during migration");

}

app.Run();
