    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RecipeSharingPlatform.Data;
    using RecipeSharingPlatform.Services.Core;
    using RecipeSharingPlatform.Services.Core.Contracts;

    namespace RecipeSharingPlatform.Web
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                builder.Services.Configure<IdentityOptions>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    //builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");  
                    options.Password.RequireDigit =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
                    options.Password.RequireNonAlphanumeric =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                    options.Password.RequireUppercase =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                    options.Password.RequireLowercase =
                        builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                    options.Password.RequiredLength =
                        builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
                });
                builder.Services.AddControllersWithViews();
                
                builder.Services.AddScoped<IRecipeService, RecipeService>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                app.MapRazorPages();

                app.Run();
            }
        }
    }
