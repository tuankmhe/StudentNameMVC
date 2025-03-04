using BusinessObject.Repositories;
using BusinessObject.Repository;
using BusinessObject.Services;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentName_ClassCode_A01
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Cấu hình DbContext với chuỗi kết nối từ appsettings.json
            services.AddDbContext<FunewsManagementContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FUNewsManagementConnection")));

            // Đăng ký Distributed Memory Cache và Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Đăng ký MVC Controllers với Views
            services.AddControllersWithViews();

            // Đăng ký DI cho Repository và các Service
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Sử dụng Session (phải gọi trước UseEndpoints)
            app.UseSession();
            app.UseAuthorization();
            // Nếu không dùng Authentication/Authorization dựa trên cookie, ta bỏ qua app.UseAuthentication() và app.UseAuthorization()

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
