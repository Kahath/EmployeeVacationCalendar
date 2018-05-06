using AutoMapper;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Domain.Service;
using EmployeeVacationCalendar.Repository.Context;
using EmployeeVacationCalendar.Repository.Core;
using EmployeeVacationCalendar.Service.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using React.AspNet;
using System;

namespace EmployeeVacationCalendar.Web
{
	/// <summary>
	/// Main startup configuration.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// <see cref="Startup"/> constructor.
		/// </summary>
		/// <param name="configuration">Injected <see cref="IConfiguration"/> type.</param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// Startup configuration instance.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"><see cref="IServiceCollection"/> parameter.</param>
		/// <returns>Configured <see cref="IServiceCollection"/> instance.</returns>
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddAutoMapper();
			services.AddDbContext<ApplicationContext>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<ISecurityService, SecurityService>();
			services.AddTransient<ICalendarRepository, CalendarRepository>();
			services.AddTransient<IEmployeeRepository, EmployeeRepository>();
			services.AddTransient<IVacationRepository, VacationRepository>();

			services.AddAuthentication(o =>
			{
				o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			}).AddCookie(options =>
			{
				options.AccessDeniedPath = new PathString("/Account/Login/");
				options.LoginPath = new PathString("/Account/Login/");
			});

			services.AddReact();
			services.AddMvc();

			return services.BuildServiceProvider();
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"><see cref="IApplicationBuilder"/> parameter.</param>
		/// <param name="env"><see cref="IHostingEnvironment"/> parameter.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			var context = app.ApplicationServices.GetService<ApplicationContext>();
			context.Database.Migrate();

			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseReact(config =>
			{
				config.UseServerSideRendering = false;
			});


			app.UseAuthentication();
			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Calendar}/{action=Index}/{id?}");
			});
		}
	}
}
