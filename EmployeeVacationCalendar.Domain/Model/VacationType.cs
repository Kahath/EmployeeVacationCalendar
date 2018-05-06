namespace EmployeeVacationCalendar.Domain.Model
{
	/// <summary>
	/// Vacation type.
	/// </summary>
	public enum VacationType
	{
		/// <summary>
		/// Default value.
		/// </summary>
		None,

		/// <summary>
		/// Vacation.
		/// </summary>
		Vacation,

		/// <summary>
		/// Vacation due to sick.
		/// </summary>
		Sick,

		/// <summary>
		/// Vacation due to leave.
		/// </summary>
		Leave,
	};
}
