using EmployeeVacationCalendar.Web.Globalization;
using System.ComponentModel.DataAnnotations;

namespace EmployeeVacationCalendar.Web.Models
{
	/// <summary>
	/// Login viewModel from login view.
	/// </summary>
	public class LoginViewModel
	{
		#region Properties

		/// <summary>
		/// Username from login view.
		/// </summary>
		[Display(Name = nameof(Resources.UserName), ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceName = nameof(Resources.RequiredUserName), ErrorMessageResourceType = typeof(Resources))]
		public string Username { get; set; }

		/// <summary>
		/// Password from login view.
		/// </summary>
		[Display(Name = nameof(Resources.Password), ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceName = nameof(Resources.RequiredPassword), ErrorMessageResourceType = typeof(Resources))]
		public string Password { get; set; }

		#endregion
	}
}
