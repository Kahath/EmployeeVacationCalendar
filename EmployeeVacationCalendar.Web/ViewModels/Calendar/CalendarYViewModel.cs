using System.Collections.Generic;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// Y axis of react rendered calendar.
	/// </summary>
	public class CalendarYViewModel
    {
		/// <summary>
		/// CalendarY Id (UserId).
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// CalendarY name (<see cref="CalendarYViewModel"/> name).
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Is editable.
		/// </summary>
		public bool CanEdit { get; set; }

		/// <summary>
		/// Associated <see cref="WeekViewModel"/> collection.
		/// </summary>
		public IEnumerable<WeekViewModel> Weeks { get; set; }
    }
}
