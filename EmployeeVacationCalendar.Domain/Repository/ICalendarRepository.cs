using EmployeeVacationCalendar.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Domain.Repository
{
	/// <summary>
	/// Calendar repository interface.
	/// </summary>
	public interface ICalendarRepository
    {
		/// <summary>
		/// Gets collection of calendar days.
		/// </summary>
		/// <param name="year">Year parameter</param>
		/// <param name="month">Month parameter.</param>
		/// <returns><see cref="CalendarDay"/> collection.</returns>
		Task<IEnumerable<CalendarDay>> FindAsync(int year, int month);
	}
}
