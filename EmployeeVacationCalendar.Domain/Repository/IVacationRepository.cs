using EmployeeVacationCalendar.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Domain.Repository
{
	/// <summary>
	/// Vacation repository interface.
	/// </summary>
	public interface IVacationRepository
    {
		/// <summary>
		/// Adds vacation for employee.
		/// </summary>
		/// <param name="vacation"><see cref="Vacation"/> parameter.</param>
		/// <returns>Added vacation identifier.</returns>
		Task<int> AddAsync(Vacation vacation);

		/// <summary>
		/// Updates vacation for employee.
		/// </summary>
		/// <param name="vacation"><see cref="Vacation"/> parameter.</param>
		/// <returns>True if successful.</returns>
		Task<bool> UpdateAsync(Vacation vacation);

		/// <summary>
		/// Gets vacation by id.
		/// </summary>
		/// <param name="Id">Id parameter.</param>
		/// <returns><see cref="Vacation"/> type.</returns>
		Task<Vacation> GetAsync(int Id);

		/// <summary>
		/// Removes vacation by id.
		/// </summary>
		/// <param name="Id">Id parameter.</param>
		/// <returns>True if successful.</returns>
		Task<bool> RemoveAsync(int Id);
    }
}
