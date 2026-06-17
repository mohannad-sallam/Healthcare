using FluentValidation;
using Healthcare.Api.Middleware;
using Healthcare.Api.Services;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Application.Common.Behaviors;
using Healthcare.Application.Features.Auth.Commands;
using Healthcare.Infrastructure.Options;
using Healthcare.Infrastructure.Persistence;
using Healthcare.Infrastructure.Repositories;
using Healthcare.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(SignupCommand).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(SignupCommandValidator).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IJwtTokenService, JWTTokenService>();
builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions!.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IWebhookEndpointRepository, WebhookEndpointRepository>();
builder.Services.AddScoped<IWebhookDeliveryLogRepository, WebhookDeliveryLogRepository>();
builder.Services.AddScoped<IWebhookService, WebhookService>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// add swagger with jwt authentication support

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    const string securitySchemeName = "Bearer";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Healthcare API",
        Version = "v1",
        Description = "Healthcare Mini Assignment API"
    });

    options.AddSecurityDefinition(securitySchemeName, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Paste only the token without writing 'Bearer'.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document =>
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference(securitySchemeName, document),
                new List<string>()
            }
        };
    });
});


// allow angular frontend to access the api
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
