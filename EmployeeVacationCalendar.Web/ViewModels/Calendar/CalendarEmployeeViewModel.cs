using System.Collections.Generic;

namespace EmployeeVacationCalendar.Web.ViewModels.Calendar
{
	/// <summary>
	/// CalendarEmployee used for mapping domain model.
	/// </summary>
	public class CalendarEmployeeViewModel
	{
		/// <summary>
		/// Employee id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Employee UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Employee first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Employee last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Employee name.
		/// Combinde first and last name.
		/// </summary>
		public string Name => $"{FirstName} {LastName}";

		/// <summary>
		/// <see cref="CalendarVacationViewModel"/> collection.
		/// Employee associated vacations.
		/// </summary>
		public IEnumerable<CalendarVacationViewModel> Vacations { get; set; }
	}
}
