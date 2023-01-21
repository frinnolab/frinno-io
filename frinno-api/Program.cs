using System.Text;
using frinno_application.Authentication;
using frinno_application.Generics;
using frinno_infrastructure;
using frinno_infrastructure.Data;
using frinno_infrastructure.Repostories;
using frinno_infrastructure.Repostories.AuthRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer("name=ConnectionStrings:frinnordb"));
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("FRINNODB"));
// builder.Services.AddDbContext<MockDataContext>(options => options.UseInMemoryDatabase("FRINNODB"));// MockDB

builder.Services.AddScoped<IAuthService, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenRepository>();

// builder.Services.AddIdentity<MockUser, IdentityRole>()
//     .AddEntityFrameworkStores<MockUserContext>()
//     .AddDefaultTokenProviders();

//Setup Auth
builder.Services.AddAuthentication(authOptions => {
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOptions=>{
    var key = builder.Configuration.GetSection("MockSettings:MockApiKey").ToString();
    jwtOptions.TokenValidationParameters = new TokenValidationParameters ()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = "FrinnoIO",
        ValidIssuer = "FrinnoIO",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5HMQ@FbiMTkWu6m"))
    };
});


builder.Services.AddSwaggerGen(c=>{
        // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
