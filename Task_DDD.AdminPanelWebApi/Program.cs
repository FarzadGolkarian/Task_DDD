using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Task_DDD.AdminPanelWebApi.StartupExtensions;
using Task_DDD.EF.DatabaseContext;

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

      ;

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
