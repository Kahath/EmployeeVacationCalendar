using System;

namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// Calendar day domain model.
	/// </summary>
	public class CalendarDay
	{
		/// <summary>
		/// Calendar day date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Calendar day year.
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// Calendar day quarter of year.
		/// </summary>
		public int Quarter { get; set; }

		/// <summary>
		/// Calendar day month of year.
		/// </summary>
		public int Month { get; set; }

		/// <summary>
		/// Calendar day week of year.
		/// </summary>
		public int Week { get; set; }

		/// <summary>
		/// Calendar day of month.
		/// </summary>
		public int Day { get; set; }

		/// <summary>
		/// Calendar day of year.
		/// </summary>
		public int DayOfYear { get; set; }

		/// <summary>
		/// Calendar day of week.
		/// </summary>
		public int DayOfWeek { get; set; }

		/// <summary>
		/// Calendar week of month.
		/// </summary>
		public int WeekOfMonth { get; set; }

		/// <summary>
		/// Is calendar day holiday.
		/// </summary>
		public bool IsHoliday { get; set; }

		/// <summary>
		/// Holiday name
		/// </summary>
		public string Description { get; set; }
	}
}
