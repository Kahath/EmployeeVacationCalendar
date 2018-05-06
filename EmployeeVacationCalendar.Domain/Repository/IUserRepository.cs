using EmployeeVacationCalendar.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Domain.Repository
{
	/// <summary>
	/// User repository interface.
	/// </summary>
	public interface IUserRepository
    {
		/// <summary>
		/// Gets user.
		/// </summary>
		/// <param name="Id">Id parameter.</param>
		/// <returns><see cref="User"/> type.</returns>
		Task<User> GetByIdAsync(int Id);

		/// <summary>
		/// Gets user
		/// </summary>
		/// <param name="userName">Username parameter.</param>
		/// <returns><see cref="User"/> type.</returns>
		Task<User> GetByUserNameAsync(string userName);

		/// <summary>
		/// Creates user.
		/// </summary>
		/// <param name="user"><see cref="User"/> type.</param>
		/// <returns>Added user identifier.</returns>
		Task<int> AddAsync(User user);

		/// <summary>
		/// Adds role to user.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		/// <param name="roleName">Role name parameter.</param>
		Task<bool> AddRoleAsync(int userId, string roleName);

		/// <summary>
		/// Gets all user roles.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
	}
}
