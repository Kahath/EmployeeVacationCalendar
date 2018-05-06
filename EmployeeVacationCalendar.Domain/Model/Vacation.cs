using System;

namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// Vacation domain model.
	/// </summary>
	public class Vacation
	{
		/// <summary>
		/// Vacation Id.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// Vacation employee id.
		/// </summary>
		public int EmployeeId { get; set; }

		/// <summary>
		/// <see cref="VacationStatus"/> status.
		/// Not used.
		/// </summary>
		public VacationStatus Status { get; set; }

		/// <summary>
		/// <see cref="VacationType"/> type.
		/// </summary>
		public VacationType Type { get; set; }

		/// <summary>
		/// Vacation start date.
		/// </summary>
		public DateTime DateFrom { get; set; }

		/// <summary>
		/// Vacation end date.
		/// </summary>
		public DateTime DateTo { get; set; }

		/// <summary>
		/// Vacation employee.
		/// </summary>
		public Employee Employee { get; set; }
	}
}
