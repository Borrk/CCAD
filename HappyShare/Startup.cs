using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HappyShare.Data;
using HappyShare.Models;
using HappyShare.Services;

namespace HappyShare
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
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // connect to DB: 
            //    local SQL server: "SQLServer"
            //    PostgreSQL: "PostgreSQL"
            DBConnectorFactory.Connect2DB(Configuration.GetSection("UsedDatabase")["Default"], services, Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(20);
                option.Cookie.HttpOnly = true;
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            // enable session
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            await CreateRoles(serviceProvider);
        }

        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var apContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                apContext.Database.EnsureCreated();

                //If there is already an Administrator role, abort
                var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                string[] roleNames = { "Admin", "Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {

                    bool roleExist = _roleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist)
                    {
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }

                }

                var poweruser = new ApplicationUser
                {
                    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"],
                    Address = "Addmin Address"
                    ,
                    Enabled = true
                };
                var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var test = _userManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);
                if (test.Result == null)
                {
                    string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
                    poweruser.EmailConfirmed = true;
                    var createPowerUser = await _userManager.CreateAsync(poweruser, UserPassword);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the "Admin" role 
                        await _userManager.AddToRoleAsync(poweruser, "Admin");
                    }
                }
            }
        }

    }
}
