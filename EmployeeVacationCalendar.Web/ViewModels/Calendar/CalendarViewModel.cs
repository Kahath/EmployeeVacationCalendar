using EmployeeVacationCalendar.Web.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// This view model is used in react calendar EmployeesCalendar as entry object.
	/// </summary>
	public class CalendarViewModel
	{
		/// <summary>
		/// Month number to string map.
		/// </summary>
		private static readonly Dictionary<int, string> _monthsMapping = new Dictionary<int, string>()
		{
			{ 1, Resources.January },
			{ 2, Resources.February },
			{ 3, Resources.March },
			{ 4, Resources.April },
			{ 5, Resources.May },
			{ 6, Resources.June },
			{ 7, Resources.July },
			{ 8, Resources.August },
			{ 9, Resources.September },
			{ 10, Resources.October },
			{ 11, Resources.November },
			{ 12, Resources.December },
		};

		/// <summary>
		/// Max number of pages.
		/// </summary>
		public int MaxPages { get; set; }

		/// <summary>
		/// Used on view as Y component of calendar.
		/// </summary>
		public IEnumerable<CalendarYViewModel> CalendarY { get; set; }

		/// <summary>
		/// Composes given parameters into view-ready calendar data.
		/// </summary>
		/// <param name="employees">Employees on Y axis.</param>
		/// <param name="calendarDays">Calendar days per employee.</param>
		/// <param name="userId">UserId used for click enabling.</param>
		/// <param name="isAdmin">Is user admin. Admin can change everything.</param>
		/// <param name="maxPages">Calendar max pages.</param>
		public CalendarViewModel(IEnumerable<CalendarEmployeeViewModel> employees, IEnumerable<CalendarDayViewModel> calendarDays, int userId, bool isAdmin, int maxPages)
		{
			MaxPages = maxPages;

			if (employees != null)
			{
				List<CalendarYViewModel> calendarY = new List<CalendarYViewModel>(employees.Count());
				// Logged in user is always first on list
				foreach (CalendarEmployeeViewModel employee in employees.OrderByDescending(x => x.UserId == userId))
				{
					bool canEdit = userId == employee.UserId || isAdmin;
					IEnumerable<CalendarDayViewModel> days = calendarDays.Select(x => new CalendarDayViewModel(x)).ToList();

					CalendarYViewModel y = new CalendarYViewModel() { Id = employee.Id, Name = employee.Name, CanEdit = canEdit };
					SetVacationDays(employee.Vacations, days, canEdit);

					IEnumerable<int> weeks = days.Select(x => x.Week).Distinct();

					y.Weeks = GetWeeks(days.First().Month, weeks, days);
					calendarY.Add(y);
				}

				CalendarY = calendarY;
			}
		}

		/// <summary>
		/// Composes one month days into weeks -> days data structure.
		/// </summary>
		/// <param name="month">Month parameter.</param>
		/// <param name="weeks">Distinct number of weeks in month.</param>
		/// <param name="days">Days in month.</param>
		/// <returns></returns>
		private IEnumerable<WeekViewModel> GetWeeks(int month, IEnumerable<int> weeks, IEnumerable<CalendarDayViewModel> days)
		{
			List<WeekViewModel> retVal = new List<WeekViewModel>();
			foreach (int week in weeks)
			{
				WeekViewModel wvm = new WeekViewModel() { Id = week };
				wvm.Days = days.Where(x => x.Week == week && x.Month == month);
				retVal.Add(wvm);
			}

			return retVal;
		}

		/// <summary>
		/// Sets vacations sata for each day by given days and vacations.
		/// Enables/Disables edit for days.
		/// </summary>
		/// <param name="vacations">Vacations parameter.</param>
		/// <param name="days">Days parameter.</param>
		/// <param name="canEdit">Can days be updated.</param>
		private void SetVacationDays(IEnumerable<CalendarVacationViewModel> vacations, IEnumerable<CalendarDayViewModel> days, bool canEdit)
		{
			foreach(CalendarDayViewModel day in days)
			{
				CalendarVacationViewModel vacation = vacations.FirstOrDefault(x => x.DateFrom <= day.Date && x.DateTo >= day.Date && !day.IsHoliday && !day.IsWeekend);
				if (vacation != null)
				{
					day.VacationId = vacation.Id.GetValueOrDefault();
					day.VacationStatus = vacation.Status;
					day.VacationType = vacation.Type;
				}

				day.CanEdit = canEdit && !day.IsHoliday && !day.IsWeekend;
			}
		}
	}
}
