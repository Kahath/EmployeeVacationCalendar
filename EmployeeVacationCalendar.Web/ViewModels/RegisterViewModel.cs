using EmployeeVacationCalendar.Web.Globalization;
using System.ComponentModel.DataAnnotations;

namespace EmployeeVacationCalendar.Web.ViewModels
{
	/// <summary>
	/// Register view model from register view.
	/// </summary>
	public class RegisterViewModel
	{
		private const string PasswordRegex = @"^(?=.{3,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).\S*$";
		private const int MinPasswordLength = 4;

		/// <summary>
		/// Username from register view.
		/// </summary>
		[Display(Name = nameof(Resources.UserName), ResourceType = typeof(Resources))]
		[Required(ErrorMessageResourceName = nameof(Resources.RequiredUserName), ErrorMessageResourceType = typeof(Resources))]
		public string Username { get; set; }

		/// <summary>
		/// Password from register view.
		/// Password must be at least 4 characters long.
		/// Password must have at least 1 capital letter and one number.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.RequiredPassword), ErrorMessageResourceType = typeof(Resources))]
		[Display(Name = nameof(Resources.Password), ResourceType = typeof(Resources))]
		[MinLength(MinPasswordLength, ErrorMessageResourceName = nameof(Resources.RequiredPasswordLength), ErrorMessageResourceType = typeof(Resources))]
		//[RegularExpression(PasswordRegex, ErrorMessageResourceName = nameof(Resources.RequiredPasswordRegex), ErrorMessageResourceType = typeof(Resources))]
		public string Password { get; set; }

		/// <summary>
		/// PasswordConfirm from register view.
		/// PasswordConfirm must match Password property.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.RequiredPassword), ErrorMessageResourceType = typeof(Resources))]
		[Display(Name = nameof(Resources.PasswordConfirm), ResourceType = typeof(Resources))]
		[MinLength(MinPasswordLength, ErrorMessageResourceName = nameof(Resources.RequiredPasswordLength), ErrorMessageResourceType = typeof(Resources))]
		[Compare(nameof(Password), ErrorMessageResourceName = nameof(Resources.PasswordMatch), ErrorMessageResourceType = typeof(Resources))]
		//[RegularExpression(PasswordRegex, ErrorMessageResourceName = nameof(Resources.RequiredPasswordRegex), ErrorMessageResourceType = typeof(Resources))]
		public string PasswordConfirm { get; set; }

		/// <summary>
		/// First name from register view.
		/// </summary>
		[Display(Name = nameof(Resources.FirstName), ResourceType = typeof(Resources))]
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.RequiredFirstName), ErrorMessageResourceType = typeof(Resources))]
		public string FirstName { get; set; }

		/// <summary>
		/// Last name from register view.
		/// </summary>
		[Display(Name = nameof(Resources.LastName), ResourceType = typeof(Resources))]
		[Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Resources.RequiredLastName), ErrorMessageResourceType = typeof(Resources))]
		public string LastName { get; set; }
	}
}
