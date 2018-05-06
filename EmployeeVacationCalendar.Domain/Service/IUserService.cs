using EmployeeVacationCalendar.Domain.Model;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Domain.Service
{
	/// <summary>
	/// User service interface.
	/// </summary>
	public interface IUserService
    {
		/// <summary>
		/// Adds new user to application.
		/// </summary>
		/// <param name="user"><see cref="User"/> parameter.</param>
		/// <param name="employee"><see cref="Employee"/> parameter.</param>
		/// <returns>True if successful.</returns>
		Task<bool> AddAsync(User user, Employee employee);
    }
}
