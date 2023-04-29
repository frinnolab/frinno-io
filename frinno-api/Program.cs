using System.Text;
using frinno_application.Articles;
using frinno_application.Authentication;
using frinno_application.Skills;
using frinno_application.Generics;
using frinno_application.Profiles;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Skill;
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
using frinno_infrastructure.Repostories.ArticlesRepositories;
using frinno_infrastructure.Repostories.SkillsRepositories;
using frinno_application.Tags;
using frinno_core.Entities.Tags;
using frinno_infrastructure.Repostories.TagsRepository;
using frinno_core.Entities.user;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Config Environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("FRINNODB"));
    // builder.Services.AddDbContext<DataContext>(
    //     options=>
    //     options.UseSqlServer(builder.Configuration.GetConnectionString("frinnoldb")));
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<DataContext>(
        options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("frinnordb")));
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:ApiKey"]))
    };
});

builder.Services.AddCors(pt=>{
    pt.AddPolicy("Cors", 
        builder =>{
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Frinno-LAB API", Version = "v1" });
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
            new string[]{}
        }
    });
});

builder.Services.AddScoped<IAuthService, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenRepository>();
builder.Services.AddScoped<IProfileService<Profile>, ProfileRepository>();
builder.Services.AddScoped<IProjectsManager<Project>, ProjectsRepository>();
builder.Services.AddScoped<IArticlesService<Article>, ArticlesRepository>();
builder.Services.AddScoped<ISkillsService, SkillsRepository>();
builder.Services.AddScoped<ITagsService<Tag>, TagsRepository>();

builder.Services.AddControllers();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseCors("Cors");
app.UseHttpsRedirection();

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


app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
