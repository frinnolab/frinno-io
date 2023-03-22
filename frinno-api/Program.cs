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

var builder = WebApplication.CreateBuilder(args);

//Setup Auth
builder.Services.AddAuthentication(p=>{
    p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(pt=>{
    pt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = "Frinno-IO",
        ValidAudience = "Frinno-IO",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5HMQ@FbiMTkWu6m5HMQ@FbiMTkWu6m"))
    };
});

builder.Services.AddCors(pt=>{
    pt.AddPolicy("Cors", builder =>{
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
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Frinno-IO API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Token invalid, Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
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

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("FRINNODB"));
    // builder.Services.AddDbContext<DataContext>(options=>
    //     options.UseSqlServer(builder.Configuration.GetConnectionString("frinnoldb")));
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<DataContext>(options=>
        options.UseSqlServer(builder.Configuration.GetConnectionString("frinnordb")));
}

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

app.UseAuthentication();
app.UseAuthorization();
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

app.UseStaticFiles();

app.Run();
