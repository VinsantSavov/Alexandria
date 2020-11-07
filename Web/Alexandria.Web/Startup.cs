﻿namespace Alexandria.Web
{
    using System;
    using System.Reflection;

    using Alexandria.Data;
    using Alexandria.Data.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Seeding;
    using Alexandria.Services.Authors;
    using Alexandria.Services.Awards;
    using Alexandria.Services.Books;
    using Alexandria.Services.Countries;
    using Alexandria.Services.EditionLanguages;
    using Alexandria.Services.Genres;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.Messaging;
    using Alexandria.Services.Reviews;
    using Alexandria.Services.Tags;
    using Alexandria.Services.Users;
    using Alexandria.Web.ViewModels;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AlexandriaDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            // services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                // .AddRoles<ApplicationRole>().AddEntityFrameworkStores<AlexandriaDbContext>();
            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Application services

            // services.AddTransient<IMapper, Mapper>();
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IAuthorsService, AuthorsService>();
            services.AddTransient<IAwardsService, AwardsService>();
            services.AddTransient<IBooksService, BooksService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<IEditionLanguagesService, EditionLanguagesService>();
            services.AddTransient<IGenresService, GenresService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IUsersService, UsersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            /*using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AlexandriaDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }*/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
