using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApi.Middleware;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementBusinessLayer.Services.Auth;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Data;
using QuantityMeasurementRepository.Services;

var builder = WebApplication.CreateBuilder(args);

// EF Core -> SQL Server ---------------------------------------------------
// AppDbContext lives in Repository layer. Every Swagger API call saves here.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("QuantityMeasurementApi")));

// -- Dependency Injection ---------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();

// EfQuantityMeasurementRepository -> all Swagger measurements go to SQL Server
builder.Services.AddScoped<EfQuantityMeasurementRepository>();
builder.Services.AddScoped<IQuantityMeasurementRepository>(
    sp => sp.GetRequiredService<EfQuantityMeasurementRepository>());

// UC1-UC16 business logic - completely unchanged
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

// -- JWT Authentication -----------------------------------------------------
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// --  Swagger ----------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1",
        Description = "UC17 REST API. " +
                      "1) Register → 2) Login → 3) Click Authorize → paste Bearer token → 4) Test endpoints. " +
                      "Every operation is saved to SQL Server automatically."
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your-token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// --Ensure SQL Server tables exist on startup ------------------------------
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("[UC17] SQL Server tables ready.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[UC17] DB warning: {ex.Message}");
    }
}

// --  Middleware pipeline ----------------------------------------------------
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();