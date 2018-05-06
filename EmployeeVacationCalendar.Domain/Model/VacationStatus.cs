namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// Vacation status enumeration.
	/// </summary>
	public enum VacationStatus
    {
		/// <summary>
		/// Default value.
		/// </summary>
		None = 0,

		/// <summary>
		/// Submitted vacation status.
		/// </summary>
		Submitted = 1,

		/// <summary>
		/// Approved vacation status.
		/// </summary>
		Approved = 2,

		/// <summary>
		/// Rejected vacation status
		/// </summary>
		Rejected = 3,
    }
}
