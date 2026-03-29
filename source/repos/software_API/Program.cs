using Microsoft.EntityFrameworkCore;
using software_API.Data;
using software_API.Middleware;
using software_API.Services;

namespace software_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var env = builder.Environment;

            // Add DbContext - Use PostgreSQL in Production, SQL Server in Development
            builder.Services.AddDbContext<YadElawnContext>(options =>
            {
                if (env.IsProduction())
                {
                    // PostgreSQL for Railway
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                else
                {
                    // SQL Server for Local Development
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
            });

            // Add Services
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<DatabaseTestService>();

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // ????? ?????????? ??? ????? ????? ??? JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Logging
            builder.Services.AddLogging();

            var app = builder.Build();

            // Use Global Exception Handling Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Only show Swagger in Development
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
