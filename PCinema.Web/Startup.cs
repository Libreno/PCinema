using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PCinema.Web.Controllers;
using VueCliMiddleware;

namespace PCinema.Web
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
			var elasticHost = Configuration.GetValue<string>("elasticHost");
			var elasticUser = Configuration.GetValue<string>("elasticUser");
			var elasticPassw = Configuration.GetValue<string>("elasticPassw");
			services.AddTransient(sp => new SearchRepository(elasticHost, elasticUser, elasticPassw, sp.GetService<ILogger<SearchRepository>>()));
			services.AddControllers(options => {
				options.ModelBinderProviders.Insert(0, new PersonEntityBinderProvider());
			});
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseSpaStaticFiles();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSpa(spa =>
			{
				if (env.IsDevelopment())
					spa.Options.SourcePath = "ClientApp";
				else
					spa.Options.SourcePath = "dist";

				if (env.IsDevelopment())
				{
					spa.UseVueCli(npmScript: "serve");
				}

			});
		}
	}
}
