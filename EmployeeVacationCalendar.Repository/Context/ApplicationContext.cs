using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EmployeeVacationCalendar.Repository.Context
{
	/// <summary>
	/// Main database context of application.
	/// Connection string is configured in appsettings.json under "ConnectionStrings:DefaultConnection" key
	/// </summary>
	public class ApplicationContext : DbContext
	{
		/// <summary>
		/// <see cref="User"/> collection from database
		/// </summary>
		public DbSet<User> Users { get; set; }

		/// <summary>
		/// <see cref="Role"/> collection from database.
		/// </summary>
		public DbSet<Role> Roles { get; set; }

		/// <summary>
		/// <see cref="UserRole"/> collection from database.
		/// </summary>
		public DbSet<UserRole> UserRoles { get; set; }

		/// <summary>
		/// <see cref="CalendarDay"/> collection from database.
		/// </summary>
		public DbSet<CalendarDay> Calendar { get; set; }

		/// <summary>
		/// <see cref="Employee"/> collection from database.
		/// </summary>
		public DbSet<Employee> Employees { get; set; }

		/// <summary>
		/// <see cref="Vacation"/> collection from database.
		/// </summary>
		public DbSet<Vacation> Vacations { get; set; }

		/// <summary>
		/// Configures context using sqlServer provider and connection string from configuration.
		/// </summary>
		/// <param name="optionsBuilder">Context options.</param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationBuilder builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json");

				IConfigurationRoot config = builder.Build();
				optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
			}

			base.OnConfiguring(optionsBuilder);
		}

		/// <summary>
		/// Model creating logic.
		/// </summary>
		/// <param name="modelBuilder"><see cref="ModelBuilder"/> parameter.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasKey(x => x.Id);
			modelBuilder.Entity<User>().HasOne(x => x.Employee);

			modelBuilder.Entity<Role>().HasKey(x => x.Id);

			modelBuilder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });

			modelBuilder.Entity<Employee>().HasKey(x => x.Id);

			modelBuilder.Entity<Vacation>().HasKey(x => x.Id);
			modelBuilder.Entity<Vacation>().HasOne(x => x.Employee).WithMany(x => x.Vacations).HasForeignKey(x => x.EmployeeId).IsRequired();

			modelBuilder.Entity<CalendarDay>().HasKey(x => x.Date);

			base.OnModelCreating(modelBuilder);
		}
	}
}
