﻿using System.Text.Json.Serialization;
using ThucHanhAPI2.Data;
using Microsoft.EntityFrameworkCore;
using ThucHanhAPI2.Repositories;
using ThucHanhAPI2.Respository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

var _logger = new LoggerConfiguration()
 .WriteTo.Console()
 .WriteTo.File("Logs/Book_log.txt", rollingInterval: RollingInterval.Minute)
 .MinimumLevel.Information() 
 .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(_logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Book API",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new
   OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
 {
 new OpenApiSecurityScheme
 {
 Reference= new OpenApiReference
 {
 Type= ReferenceType.SecurityScheme,
 Id= JwtBearerDefaults.AuthenticationScheme
 },
 Scheme = "Oauth2",
 Name =JwtBearerDefaults.AuthenticationScheme,
 In = ParameterLocation.Header
 },
 new List<string>()
 }
 });
});
// Register database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<BookAuthDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("BookAuthConnection")));
builder.Services.AddScoped<IPublisherRepository, SQLPublisherRepository>();
builder.Services.AddScoped<IAuthorRepository, SQLAuthorRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
// config identity user
builder.Services.AddIdentityCore<IdentityUser>()
 .AddRoles<IdentityRole>()
 .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Book")
 .AddEntityFrameworkStores<BookAuthDbContext>()
 .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true, 
    ValidateLifetime = true, 
    ValidateIssuerSigningKey = true, 
    ValidIssuer = builder.Configuration["Jwt:Issuer"], 
    ValidAudience = builder.Configuration["Jwt:Audience"], 
    ClockSkew = TimeSpan.Zero, 
    IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
 });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
