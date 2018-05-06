using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Domain.Service;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Service.Core
{
	/// <summary>
	/// User service used for registration only for now.
	/// </summary>
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IEmployeeRepository _employeeRepository;

		/// <summary>
		/// <see cref="UserService"/> constructor.
		/// </summary>
		/// <param name="userRepository">Injected <see cref="IUserRepository"/> parameter.</param>
		/// <param name="employeeRepository">Injected <see cref="IEmployeeRepository"/> parameter.</param>
		public UserService(IUserRepository userRepository, IEmployeeRepository employeeRepository)
		{
			_userRepository = userRepository;
			_employeeRepository = employeeRepository;
		}

		/// <summary>
		/// Registers new user by given object.
		/// Creates new <see cref="Employee"/> by given first and last name.
		/// </summary>
		/// <param name="user"><see cref="User"/> parameter.</param>
		/// <param name="employee"><see cref="Employee"/> parameter.</param>
		/// <returns>True if succeed.</returns>
		public async Task<bool> AddAsync(User user, Employee employee)
		{
			user.Id = await _userRepository.AddAsync(user);
			if (user.Id == 0)
				return false;

			if (!await _userRepository.AddRoleAsync(user.Id.GetValueOrDefault(), Roles.User))
				return false;

			employee.UserId = user.Id.Value;

			if(await _employeeRepository.AddAsync(employee) == 0)
				return false;

			return true;
		}
	}
}
