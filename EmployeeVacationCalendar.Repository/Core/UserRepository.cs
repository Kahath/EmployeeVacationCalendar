using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Repository.Context;
using EmployeeVacationCalendar.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Repository.Core
{
	/// <summary>
	/// User data manipulation.
	/// </summary>
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="UserRepository"/> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter</param>
		public UserRepository(IMapper mapper)
		{
			_mapper = mapper;
			_context = new ApplicationContext();
		}

		/// <summary>
		/// Returns user by given Id.
		/// </summary>
		/// <param name="Id">Id parameter.</param>
		/// <returns><see cref="User"/> object.</returns>
		public async Task<User> GetByIdAsync(int Id)
		{
			return await _context.Users.SingleOrDefaultAsync(x => x.Id == Id);
		}

		/// <summary>
		/// Gets user by given username.
		/// </summary>
		/// <param name="userName">Username parameter.</param>
		/// <returns><see cref="User"/> object.</returns>
		public async Task<User> GetByUserNameAsync(string userName)
		{
			return await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);
		}

		/// <summary>
		/// Creates new user in underlying context.
		/// </summary>
		/// <param name="user"><see cref="User"/> parameter.</param>
		public async Task<int> AddAsync(User user)
		{
			if (user.Id.GetValueOrDefault() > 0)
				throw new System.Exception();

			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			return user.Id.Value;
		}

		/// <summary>
		/// Adds user role to user by given id in underlying context.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		/// <param name="roleName">Role name parameter.</param>
		public async Task<bool> AddRoleAsync(int userId, string roleName)
		{
			User user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

			if (user != null)
			{
				Role role = await _context.Roles.SingleAsync(x => x.Name == roleName);

				if(role != null)
				{
					UserRole userRole = new UserRole()
					{
						UserId = userId,
						RoleId = role.Id,
					};

					await _context.UserRoles.AddAsync(userRole);
					await _context.SaveChangesAsync();

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets all user roles by given Id.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		/// <returns><see cref="UserRole"/> collection.</returns>
		public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
		{
			IEnumerable<UserRole> userRoles = await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();
			IEnumerable<int> roleIds = userRoles.Select(x => x.RoleId);

			return await _context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();
		}
	}
}
