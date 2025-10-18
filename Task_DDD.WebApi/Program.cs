using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task_DDD.Application.ProfileMapping;
using Task_DDD.Application.RepositoryContracts.Employees;
using Task_DDD.Application.RepositoryContracts.Tickets;
using Task_DDD.Application.RepositoryContracts.Users;
using Task_DDD.Application.ServiceContracts.Employees;
using Task_DDD.Application.ServiceContracts.Tickets;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.EF.DatabaseContext;
using Task_DDD.Repository.Employees;
using Task_DDD.Repository.Tickets;
using Task_DDD.Repository.Users;
using Task_DDD.Service.Employees;
using Task_DDD.Service.Tickets;
using Task_DDD.Service.Users;
using Task_DDD.WebApi.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJWEBearer(builder);

builder.Services.AddCors(config => config.AddPolicy("default",
    policy => policy
        .SetIsOriginAllowed(ho => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
));


builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddEndpointsApiExplorer()
       .AddHttpContextAccessor()
       .AddDbContext<TaskDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString))
       .AddScoped<DbContext, TaskDbContext>()
       .AddScoped<IUserRepository, UserRepository>()
       .AddScoped<IEmployeeRepository, EmployeeRepository>()
       .AddScoped<ITicketRepository, TicketRepository>()
       .AddScoped<IUserService, UserService>()
       .AddScoped<IEmployeeService, EmployeeService>()
       .AddScoped<ITicketService, TicketService>()

      ;


builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
builder.Services.AddSwaggerGen().AddCustomizedSwagger(builder.Environment);


builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IUserAuthorizedService, AuthorizeUserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
