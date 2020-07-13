using Web.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Web.Api.Graphql;
using Microsoft.Extensions.Hosting;
using Web.Services;
using AutoWrapper;
using Web.Interfaces;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>();
            services.AddScoped<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));
            services.AddScoped<ISchema, RootSchema>();
            services.AddScoped<RootQuery>();
            services.AddSingleton<AuthorType>();
            services.AddSingleton<VisualizationType>();

            services.AddScoped<IRecommenderService, RecommenderService>();
            services.AddScoped<IDatasetAnalysisService, DatasetAnalysisService>();

            services.AddCors();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:8080", "https://visual-fox.azurewebsites.net").AllowAnyHeader());
            app.UseGraphiQl("/graphql");
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions {UseApiProblemDetailsException = true});
            app.UseMvc();
        }
    }
}