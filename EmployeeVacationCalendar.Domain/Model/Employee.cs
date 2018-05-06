using System.Collections.Generic;

namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// Employee domain model.
	/// </summary>
	public class Employee
    {
		/// <summary>
		/// Employee entity Id.
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		/// Employee entity UserId.
		/// 1:1 relation.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Employee entity first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Employee entity last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// <see cref="Vacation"/> associated collection.
		/// </summary>
		public IEnumerable<Vacation> Vacations { get; set; }
    }
}
