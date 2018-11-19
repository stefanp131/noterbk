using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Noter.API.Models;
using Noter.DAL.Context;
using Noter.DAL.Entities;
using Noter.DAL.Extensions;
using Noter.DAL.Repository;
using Noter.DAL.User;

namespace Noter.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddIdentityCore<NoterUser>(options => { });
            services.AddScoped<IUserStore<NoterUser>, NoterUserStore>();

            services.AddAuthentication("cookies").AddCookie("cookies", options => options.CookieHttpOnly = false);


            var connectionString = configuration["connectionString"];
            services.AddDbContext<NoterContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<INoterRepository, NoterRepository>();


            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NoterContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseAuthentication();

            context.EnsureData();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Topic, TopicDto>().ForMember(dest => dest.CommentariesCount, opts => opts.MapFrom(src => src.Commentaries.Count));
                cfg.CreateMap<TopicForCreationDto, Topic>();
                cfg.CreateMap<Commentary, CommentaryDto>();//.ForMember(dest => dest.Created, opts => opts.MapFrom(src => src.Created.ToShortDateString()));
                cfg.CreateMap<CommentaryForCreation, Commentary>();
                cfg.CreateMap<Commentary, CommentaryForUpdate>();
                cfg.CreateMap<CommentaryForUpdate, Commentary>();
                cfg.CreateMap<Topic, TopicForUpdateDto>();
                cfg.CreateMap<TopicForUpdateDto, Topic>();
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithExposedHeaders("X-Pagination"));

            app.UseMvc();
        }
    }
}
