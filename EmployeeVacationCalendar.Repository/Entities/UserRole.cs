using System.ComponentModel.DataAnnotations;

namespace EmployeeVacationCalendar.Repository.Entities
{
	/// <summary>
	/// User roles mapping.
	/// </summary>
	public class UserRole
    {
		/// <summary>
		/// User id foreign key.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Role id foreign key.
		/// </summary>
		public int RoleId { get; set; }
    }
}
