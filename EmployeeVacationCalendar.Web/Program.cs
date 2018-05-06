using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using EmployeeVacationCalendar.Web.Helpers;


namespace EmployeeVacationCalendar.Web
{
	/// <summary>
	/// Main application class.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// PE entry point.
		/// </summary>
		/// <param name="args">args parameter.</param>
		public static void Main(string[] args)
		{
			BuildWebHost(args)
				.MigrateDatabase()
				.Run();
		}

		/// <summary>
		/// Web host builder
		/// </summary>
		/// <param name="args">args parameter.</param>
		/// <returns><see cref="IWebHost"/> instance.</returns>
		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
