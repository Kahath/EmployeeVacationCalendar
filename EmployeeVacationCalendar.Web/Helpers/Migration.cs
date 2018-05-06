using EmployeeVacationCalendar.Repository.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeVacationCalendar.Web.Helpers
{
	/// <summary>
	/// Migration extension methods.
	/// </summary>
	public static class Migration
    {
		/// <summary>
		/// Migrates application db context.
		/// </summary>
		/// <param name="webHost"><see cref="IWebHost"/> parameter.</param>
		/// <returns><see cref="IWebHost"/> type.</returns>
		public static IWebHost MigrateDatabase(this IWebHost webHost)
		{
			IServiceScopeFactory serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

			using (IServiceScope scope = serviceScopeFactory.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;
				ApplicationContext dbContext = services.GetRequiredService<ApplicationContext>();

				dbContext.Database.Migrate();
			}

			return webHost;
		}
	}
}
