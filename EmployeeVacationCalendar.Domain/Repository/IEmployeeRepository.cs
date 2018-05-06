using EmployeeVacationCalendar.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Domain.Repository
{
	/// <summary>
	/// Employee repository interface.
	/// </summary>
	public interface IEmployeeRepository
    {
		/// <summary>
		/// Creates new employee.
		/// </summary>
		/// <param name="employee"><see cref="Employee"/> type to create.</param>
		/// <returns>Created employee identity.</returns>
		Task<int> AddAsync(Employee employee);

		/// <summary>
		/// Gets employee.
		/// </summary>
		/// <param name="id">Id parameter.</param>
		/// <returns><see cref="Employee"/> type.</returns>
		Task<Employee> GetAsync(int id);

		/// <summary>
		/// Gets employee by userId.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		/// <returns><see cref="Employee"/> type.</returns>
		Task<Employee> GetByUserAsync(int userId);

		/// <summary>
		/// Gets employees by page.
		/// </summary>
		/// <param name="page">page parameter.</param>
		/// <param name="resultsPerPage">Number of results per page.</param>
		/// <returns><see cref="Employee"/> collection.</returns>
		Task<IEnumerable<Employee>> FindAsync(int page, int resultsPerPage);

		/// <summary>
		/// Updates employee.
		/// </summary>
		/// <param name="employee"><see cref="Employee"/> parameter.</param>
		/// <returns>True if successful.</returns>
		Task<bool> UpdateAsync(Employee employee);

		/// <summary>
		/// Gets employee count.
		/// </summary>
		/// <returns>Employee count.</returns>
		Task<int> CountAsync();
	}
}
