using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.API.Models;
using Movies.API.Service;
using System.Security.Cryptography.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    //MY Document API
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Test API",
        Description =" My First API",
        TermsOfService = new Uri("http://www.google.com"),
        Contact = new OpenApiContact 
        { 
            Name = "Ahmed",
            Email = "ahmedselima.dev@gmail.com",
            Url = new Uri("http://www.google.com"),

        },
        License = new OpenApiLicense 
        {
            Name = "My License",
            Url = new Uri("http://www.google.com")
        }
    });

    //Add APIKey General
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Your Jwt Key"
    });

    //Add APIKey For each Endpoint,
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               },
               Name = "Bearer",
               In = ParameterLocation.Header
           },
           new List<string>()
        }
    });
});

//Enable Cors.
builder.Services.AddCors();

//Add ConnectionString.
var Connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(Connection));

//Add Configuration for DI.
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Enable Cors.
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.UseAuthorization();

app.MapControllers();

app.Run(); 
