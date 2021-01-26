using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dummy.Blazor.Data;
using Dummy.Core;
using Dummy.Core.Models;
using Microsoft.EntityFrameworkCore;
using MvuSharp;

namespace Dummy.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContextFactory<AppDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            //Handlers
            var handlers = new HandlerRegistrar();
            handlers.Add(
                async (Request.AddUser request, AppDbContext context, CancellationToken cancellationToken) => 
                {
                await context.AddAsync(request.UserToAdd, cancellationToken);
                try
                {
                    await context.SaveChangesAsync(cancellationToken);
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
                });
            handlers.Add(
                async (Request.DeleteUser request, AppDbContext context, CancellationToken cancellationToken) =>
                {
                    var user = await context.Users.FindAsync(request.Id, cancellationToken);
                    if (user == null) return false;
                    context.Users.Remove(user);
                    await context.SaveChangesAsync(cancellationToken);
                    return true;
                });
            services.AddSingleton(handlers);
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}