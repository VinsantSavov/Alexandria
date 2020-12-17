namespace Alexandria.Web
{
    using System.Reflection;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Data.Seeding;
    using Alexandria.Services.Authors;
    using Alexandria.Services.Awards;
    using Alexandria.Services.Books;
    using Alexandria.Services.BookTags;
    using Alexandria.Services.Cloudinary;
    using Alexandria.Services.EditionLanguages;
    using Alexandria.Services.Genres;
    using Alexandria.Services.Likes;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.Messages;
    using Alexandria.Services.Messaging;
    using Alexandria.Services.Reviews;
    using Alexandria.Services.Scrapers;
    using Alexandria.Services.StarRatings;
    using Alexandria.Services.Tags;
    using Alexandria.Services.UserFollowers;
    using Alexandria.Services.Users;
    using Alexandria.Web.Hubs;
    using Alexandria.Web.InputModels.Reviews;
    using Alexandria.Web.ViewModels;
    using AspNetCoreTemplate.Data;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Authentication.OAuth;
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

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
               .AddRoles<ApplicationRole>().AddEntityFrameworkStores<AlexandriaDbContext>();
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
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSignalR();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = this.configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = this.configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = this.configuration["Authentication:Google:ClientSecret"];
                });

            // Cloudinary
            var account = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            var cloudinary = new Cloudinary(account);

            // Application services
            services.AddSingleton(this.configuration);
            services.AddSingleton(cloudinary);

            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<IAuthorsService, AuthorsService>();
            services.AddTransient<IAwardsService, AwardsService>();
            services.AddTransient<IBooksService, BooksService>();
            services.AddTransient<IEditionLanguagesService, EditionLanguagesService>();
            services.AddTransient<IGenresService, GenresService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IGoodReadsScraperService, GoodReadsScraperService>();
            services.AddTransient<IStarRatingsService, StarRatingsService>();
            services.AddTransient<ILikesService, LikesService>();
            services.AddTransient<IUserFollowersService, UserFollowersService>();
            services.AddTransient<IBookTagsService, BookTagsService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AlexandriaDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
                    endpoints.MapHub<ChatHub>("/chat");
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("search", "{controller=Books}/{action=Search}/{search?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}
