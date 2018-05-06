using System.Collections.Generic;

namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// User domain model.
	/// </summary>
	public class User
    {
		/// <summary>
		/// User id.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// User username.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// User password.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// User employee.
		/// </summary>
		public Employee Employee { get; set; }
    }
}
