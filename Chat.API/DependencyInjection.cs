using Chat.API.Authentication;
using Chat.API.Data;
using Chat.API.Entities;
using Chat.API.Errors;
using Chat.API.Services.EmailService;
using Chat.API.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Chat.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add IdentityRole
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add IOptions
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.AddOptions<JwtSettings>().BindConfiguration(nameof(JwtSettings)).ValidateDataAnnotations();

            var settings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            //Add Authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings?.Key!)),
                    ValidIssuer = settings?.Issuer,
                    ValidAudience = settings?.Audience,
                };
            });

            //Add Email Sender Service
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.AddHttpContextAccessor();

            //Add Services
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();


            // Add Fluent Validation
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            // Add Exception Handling Middleware
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
