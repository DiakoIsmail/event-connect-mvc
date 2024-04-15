using Amazon.S3;
using EventConnect.Data;
using EventConnect.Data.Auth;
using EventConnect.Entities.Like;
using EventConnect.Repositories.ChatHubRepository;
using EventConnect.Repositories.GenericRepositories;
using EventConnect.Repositories.LikeRepositories;
using EventConnect.Repositories.PostRepositories;
using EventConnect.Repositories.S3Repositories;
using EventConnect.Services.ChatHub;
using EventConnect.Services.Likes;
using EventConnect.Services.PostServices;
using EventConnect.Services.S3Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

// Create a new web application builder with the provided command-line arguments
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog as the logging provider
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Get the connection string from the configuration
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// Add a DbContext to the service container
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));

// Add API explorer endpoints
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add HttpContextAccessor to the service container
builder.Services.AddHttpContextAccessor();

// Add services to the service container
builder.Services.AddScoped<IPostServices, PostServices>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILikeServices,LikeServices>();
builder.Services.AddScoped<IPostRepositories,PostRepositories>();
builder.Services.AddScoped<ILikeRepositories,LikeRepositories>();
builder.Services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
builder.Services.AddScoped<IS3Services, S3Services >();
builder.Services.AddScoped<IS3Repository,S3Repository>();

// Add authorization
builder.Services.AddAuthorization();

// Add controllers to the service container
builder.Services.AddControllers();
// add signalR t
builder.Services.AddSignalR();
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("reactApp",  builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add ConnectionRepository to the service container
builder.Services.AddSingleton<ConnectionRepository>();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use routing
app.UseRouting();

// Use authentication
app.UseAuthentication();

// Use authorization
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map chat hubs
app.MapHub<ChatHubs>("/Chat");

// Use CORS
app.UseCors("reactApp");

// Run the application
app.Run();