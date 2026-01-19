using book_shop_back_end.Data;
using book_shop_back_end.Models;
using book_shop_back_end.Repositories;
using book_shop_back_end.Seed;        
using book_shop_back_end.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace book_shop_back_end
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.EnableRetryOnFailure()
                ));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<BookStoreContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IBookRepostiry, BookRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.AddScoped<BookRepository>();
            builder.Services.AddScoped<AuthRepository>();

            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddSingleton<DiscountService>();

            var jwtKey = builder.Configuration["Jwt:Key"]!;
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
            var jwtAudience = builder.Configuration["Jwt:Audience"]!;



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

            // CORS
            const string CorsPolicy = "Frontend";
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(CorsPolicy, p =>
                    p.WithOrigins("http://localhost:4200", "https://localhost:4200")
                     .AllowAnyHeader()
                     .AllowAnyMethod());
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
            }

            app.UseHttpsRedirection();
            app.UseCors(CorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
