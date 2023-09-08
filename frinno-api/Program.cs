using System.Text;
using frinno_application.Authentication;
using frinno_application.Generics;
using frinno_application.Profiles;
using frinno_core.Entities.Profiles;
using frinno_infrastructure;
using frinno_infrastructure.Data;
using frinno_infrastructure.Repostories;
using frinno_infrastructure.Repostories.AuthRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using frinno_application.Projects;
using frinno_core.Entities.Projects;
using frinno_infrastructure.Repostories.ProjectsRepositories;
using frinno_infrastructure.Repostories.ProfilesRepositories;
using Microsoft.Extensions.DependencyInjection;
using frinno_core.Entities.FileAsset;
using frinno_application.FileAssets;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

//Config Environment
if (builder.Environment.IsDevelopment())
{
    //builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("FRINNODB"));
    builder.Services.AddDbContext<DataContext>(
        options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("frinnotdb")));
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<DataContext>(
        options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("frinnordb")));



    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
}

//Setup Identity Store DI
builder.Services.AddIdentity<Profile, IdentityRole>()
.AddEntityFrameworkStores<DataContext>();

//Setup Auth
builder.Services.AddAuthentication(p=>{
    p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(
    pt=>
{
    pt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
        ValidAudience = builder.Configuration["AppSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:ApiKey"])),
        ClockSkew = new TimeSpan(3000)
    };
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(pt =>
{
pt.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                             .AllowAnyHeader()
                             .AllowAnyMethod();
                      });
});

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Frank Leons API  Explorer", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer ***********\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Setup services DI

builder.Services.AddScoped<IAuthService, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenRepository>();
builder.Services.AddScoped<IProfileService<Profile>, ProfileRepository>();
builder.Services.AddScoped<IProjectsManager<Project>, ProjectsRepository>();
builder.Services.AddScoped<IFileAssetService, FileAssetsRepository>();

builder.Services.AddControllers();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())  
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.Run();
