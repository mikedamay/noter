using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using noter.Common;
using noter.Data;
using noter.Services;
using static noter.Common.Utils;

namespace noter
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
//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(GetIdentityConnectionString()));
            services.AddDbContext<NoteDbContext>(options =>
                options.UseSqlServer(GetNoteConnetionString()));
//            services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDbContext>()
//                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<INoteManager, NoteManager>();
            services.AddScoped<TagService, TagService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

//            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=NoteManager}/{action=Index}/{id?}");
            });
        }

        private string GetNoteConnetionString() => GetConnetionString("Note");
        private string GetIdentityConnetionString() => GetConnetionString("Identity");
        private string GetConnetionString(string prefix)
        {
            var osNames = new Dictionary<OS, string>
            {
                {OS.Linux, "Linux"}
                ,{OS.MacOS, "MacOs"}
                ,{OS.Windows, "Windows"}
            };
            OS os = new OSDetector().DetectOS();
            Assert(osNames.ContainsKey(os));
            return Configuration.GetConnectionString($"{osNames[os]}-{prefix}Connection");
                    // e.g. ...GetconnectionString("Linux-NoteConnedtion");
        }
    }
}
