using EmployeeVacationCalendar.Domain.Model;
using System;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// Represents one calendar day.
	/// This is one day box on view.
	/// Used in CalendarDay react component.
	/// </summary>
	public class CalendarDayViewModel
	{
		/// <summary>
		/// Calendar day date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Calendar day of month.
		/// </summary>
		public int Day { get; set; }

		/// <summary>
		/// Calendar day of week.
		/// </summary>
		public int DayOfWeek { get; set; }

		/// <summary>
		/// Calendar day month.
		/// </summary>
		public int Month { get; set; }

		/// <summary>
		/// Calendar day week.
		/// </summary>
		public int Week { get; set; }

		/// <summary>
		/// Calendar day year.
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// Is calendar day holiday.
		/// </summary>
		public bool IsHoliday { get; set; }

		/// <summary>
		/// Is calendar day weekend (Saturday or Sunday).
		/// </summary>
		public bool IsWeekend => DayOfWeek == 6 || DayOfWeek == 7;

		/// <summary>
		/// Calendar day vacation id.
		/// </summary>
		public int VacationId { get; set; }

		/// <summary>
		/// Calendar day vacation status.
		/// Not used.
		/// </summary>
		public VacationStatus VacationStatus { get; set; }

		/// <summary>
		/// Calendar day <see cref="Domain.Model.VacationType"/> type
		/// </summary>
		public VacationType VacationType { get; set; }

		/// <summary>
		/// Is calendar day editable on view.
		/// </summary>
		public bool CanEdit { get; set; }

		/// <summary>
		/// Default constructor is requried for automapper.
		/// </summary>
		public CalendarDayViewModel()
		{

		}

		/// <summary>
		/// Copies CalendarDay viewModel into new reference.
		/// </summary>
		/// <param name="calendarDayModel">CalendarDay view model.</param>
		public CalendarDayViewModel(CalendarDayViewModel calendarDayModel)
		{
			Date = calendarDayModel.Date;
			Day = calendarDayModel.Day;
			DayOfWeek = calendarDayModel.DayOfWeek;
			Month = calendarDayModel.Month;
			Week = calendarDayModel.Week;
			Year = calendarDayModel.Year;
			IsHoliday = calendarDayModel.IsHoliday;
			VacationId = calendarDayModel.VacationId;
			VacationStatus = calendarDayModel.VacationStatus;
			VacationType = calendarDayModel.VacationType;
			CanEdit = calendarDayModel.CanEdit;
		}
	}
}
