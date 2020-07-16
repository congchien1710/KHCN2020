using KHCN.Data.Helpers;
using KHCN.Data.Context;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using KHCN.Data.Repository.KHCN;
using KHCN.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using KHCN.Api.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using KHCN.Service.Services;
using KHCN.Api.Provider;
using Microsoft.Extensions.Logging;

namespace KHCN.Api
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
            services.AddControllers();
            services.AddMvc();
            services.AddLogging();

            services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("KHCNAppConnection")));

            RegisterServices(services);
            RegisterJwtAuthentication(services);
            RegisterSwagger(services);
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KHCN API", Version = "v1" });
            //});

            // Enable Swagger   
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "KHCN API",
                    Description = "KHCN API"
                });

                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
        }

        private void RegisterJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile($"./Logs/KHCN-Log-{DateTime.Now.ToString("yyyy-MM-dd")}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(policy =>
            //{
            //    policy.AllowAnyHeader();
            //    policy.AllowAnyMethod();
            //    policy.AllowAnyOrigin();
            //    policy.AllowCredentials();
            //});

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                //string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                //c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "KHCN API V1");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KHCN API V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseDeveloperExceptionPage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                   name: "DefaultAPI",
                   pattern: "api/{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<FileService>();

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

            services.AddScoped<IUserProvider, UserProvider>();
            services.AddSingleton<IPathProvider, PathProvider>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICapBacRepository, CapBacRepository>();
            services.AddScoped<ICapQuanLyRepository, CapQuanLyRepository>();
            services.AddScoped<IChuanApDungRepository, ChuanApDungRepository>();
            services.AddScoped<IChucDanhRepository, ChucDanhRepository>();
            services.AddScoped<IDonViCheTaoRepository, DonViCheTaoRepository>();
            services.AddScoped<IGiaiDoanRepository, GiaiDoanRepository>();
            services.AddScoped<IGiaiDoanSanPhamRepository, GiaiDoanSanPhamRepository>();
            services.AddScoped<IHoSoDieuChinhKhacRepository, HoSoDieuChinhKhacRepository>();
            services.AddScoped<IHoSoDieuChinhKinhPhiRepository, HoSoDieuChinhKinhPhiRepository>();
            services.AddScoped<IHoSoDieuChinhThoiGianRepository, HoSoDieuChinhThoiGianRepository>();
            services.AddScoped<IHoSoNghiemThuRepository, HoSoNghiemThuRepository>();
            services.AddScoped<IHoSoQuyetToanRepository, HoSoQuyetToanRepository>();
            services.AddScoped<IHoSoXetDuyetRepository, HoSoXetDuyetRepository>();
            services.AddScoped<IKinhPhiThucHienRepository, KinhPhiThucHienRepository>();
            services.AddScoped<ILoaiNhiemVuRepository, LoaiNhiemVuRepository>();
            services.AddScoped<INganhRepository, NganhRepository>();
            services.AddScoped<INhiemVuRepository, NhiemVuRepository>();
            services.AddScoped<IPhongBanRepository, PhongBanRepository>();
            services.AddScoped<ISanPhamRepository, SanPhamRepository>();
            services.AddScoped<IThanhVienDeTaiRepository, ThanhVienDeTaiRepository>();
            services.AddScoped<IThoiGianThucHienRepository, ThoiGianThucHienRepository>();
            services.AddScoped<ITienDoThucHienRepository, TienDoThucHienRepository>();
            services.AddScoped<ITrinhDoRepository, TrinhDoRepository>();
            services.AddScoped<ITaiLieuDinhKemRepository, TaiLieuDinhKemRepository>();

        }
    }
}