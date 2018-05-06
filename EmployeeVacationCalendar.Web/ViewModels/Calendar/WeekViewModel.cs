using System.Collections.Generic;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// Used in CalendarWeek react component.
	/// </summary>
	public class WeekViewModel
	{
		/// <summary>
		/// Week id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Collection of <see cref="CalendarDayViewModel"/> week days.
		/// </summary>
		public IEnumerable<CalendarDayViewModel> Days { get; set; }
	}
}
