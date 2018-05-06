using EmployeeVacationCalendar.Domain.Model;
using System;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// Vacation entity used for view.
	/// </summary>
	public class CalendarVacationViewModel
	{
		/// <summary>
		/// Calendar vacation Id.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// Calendar vacation UserId.
		/// </summary>
		public int EmployeeId { get; set; }

		/// <summary>
		/// Calendar vacation start date.
		/// </summary>
		public DateTime DateFrom { get; set; }

		/// <summary>
		/// Calendar vacation end date.
		/// </summary>
		public DateTime DateTo { get; set; }

		/// <summary>
		/// <see cref="VacationType"/> type.
		/// </summary>
		public VacationType Type { get; set; }

		/// <summary>
		/// Calendar vacation status.
		/// Not used.
		/// </summary>
		public VacationStatus Status { get; set; }
	}
}
