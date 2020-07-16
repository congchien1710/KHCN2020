using AutoMapper;
using KHCN.Data.Context;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using KHCN.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace KHCN.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("KHCNAppConnection")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthentication("CMSSecurityScheme")
            .AddCookie("CMSSecurityScheme", options =>
            {
                options.AccessDeniedPath = new PathString("/Account/Access");
                options.Cookie = new CookieBuilder
                {
                    //Domain = "",
                    HttpOnly = true,
                    Name = ".aspNetCoreDemo.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events = new CookieAuthenticationEvents
                {
                    OnSignedIn = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnSignedIn", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnSigningOut = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnSigningOut", context.HttpContext.User.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnValidatePrincipal = context =>
                    {
                        Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                            "OnValidatePrincipal", context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    }
                };
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = new PathString("/Account/Login");
                options.ReturnUrlParameter = "RequestPath";
                options.SlidingExpiration = true;
            });

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
            services.AddAutoMapper();
             
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IFunctionRepository, FunctionRepository>();
            services.AddScoped<IApiRepository, ApiRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IRoleFunctionRepository, RoleFunctionRepository>();
            services.AddScoped<IRoleApiRepository, RoleApiRepository>();
            services.AddScoped<IRolePageRepository, RolePageRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();

            //services.AddScoped<ICapQuanLyRepository, CapQuanLyRepository>();
            //services.AddScoped<IDeTaiRepository, DeTaiRepository>();
            //services.AddScoped<IHoSoDieuChinhRepository, HoSoDieuChinhRepository>();
            //services.AddScoped<IHoSoNghiemThuRepository, HoSoNghiemThuRepository>();
            //services.AddScoped<IHoSoQuyetToanRepository, HoSoQuyetToanRepository>();
            //services.AddScoped<IHoSoXetDuyetRepository, HoSoXetDuyetRepository>();
            //services.AddScoped<IKinhPhiThucHienRepository, KinhPhiThucHienRepository>();
            //services.AddScoped<ILoaiNhiemVuRepository, LoaiNhiemVuRepository>();
            //services.AddScoped<INganhRepository, NganhRepository>();
            //services.AddScoped<IThoiGianThucHienRepository, ThoiGianThucHienRepository>();

            //services.AddScoped(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            //services.AddDbContext<KHCNDbContext>();
        }
    }
}
