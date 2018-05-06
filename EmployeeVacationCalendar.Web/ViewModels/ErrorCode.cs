namespace EmployeeVacationCalendar.Web.ViewModels
{
	/// <summary>
	/// Error enumeration for binding on view.
	/// </summary>
	public enum ErrorCode
    {
		/// <summary>
		/// Gg no re.
		/// </summary>
		Success = 0,

		/// <summary>
		/// Generic error usually from catch block.
		/// </summary>
		GenericError,

		/// <summary>
		/// Username was not found.
		/// </summary>
		UsernameNotFound,

		/// <summary>
		/// Entered password and entity password do not match.
		/// </summary>
		InvalidPassword,

		/// <summary>
		/// Username does not exist in repository.
		/// </summary>
		UsernameAlreadyExists,
    };
}
