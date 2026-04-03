using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Data;
using QuantityMeasurementRepository.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quantify API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Enter: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        Array.Empty<string>()
    }});
});

// CORS — allow Live Server on 5500 and 5501
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(
                "http://127.0.0.1:5500", "http://localhost:5500",
                "http://127.0.0.1:5501", "http://localhost:5501",
             "http://127.0.0.1:4200", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(o =>
    {
        // FIX 1: Use Lax instead of None.
        // SameSite=None requires Secure=true. On http://localhost SameAsRequest
        // sends the cookie as non-secure, so the browser silently drops it and
        // the OAuth correlation check fails with "Correlation failed."
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        };
    })
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        // FIX 2: CallbackPath must be a middleware-only path, NOT a controller route.
        // The Google middleware intercepts requests to this path itself to validate
        // the OAuth state+code. If it matches a controller route too, they conflict
        // and the correlation cookie is never validated correctly.
        // *** Also update this URI in Google Cloud Console → Authorized redirect URIs ***
        // Add: http://localhost:5001/signin-google
        o.CallbackPath = "/signin-google";
    });

builder.Services.AddAuthorization();

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IQuantityMeasurementRepository, EfQuantityMeasurementRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantify API V1"));
}

app.UseCors("AllowFrontend");   // MUST be before auth
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();