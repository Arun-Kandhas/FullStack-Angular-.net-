    using KnilServer.Application;
using KnilServer.Application.Common;
using KnilServer.Domain.Contracts;
using KnilServer.Infrastructure;
using KnilServer.Infrastructure.DbContexts;
using KnilServer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Configure Services
builder.Services.AddApplicationsService();
builder.Services.AddInfrastructureService();
#endregion

#region Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomePolicy", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
#endregion

#region Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();


#endregion

#region Configure Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);
    if(context.HostingEnvironment.IsProduction() == false)
    {
        config.WriteTo.Console();
    }

});
#endregion



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWTSetting:Issuer"],
        ValidAudience = builder.Configuration["JWTSetting:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:securitykey"]!))

    };
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = @"JWT Authorization header using the Bearer Scheme.
                        Enter 'Bearer' [space] and then your token in the input below
                        Example:'Bearer 123456789'",

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type =ReferenceType.SecurityScheme,
                    Id = "Bearer"

                },
                Scheme = "Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()

        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CustomePolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
