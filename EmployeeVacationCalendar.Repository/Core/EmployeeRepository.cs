using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EmployeeVacationCalendar.Repository.Core
{
	/// <summary>
	/// Employee data manipulation.
	/// </summary>
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="EmployeeRepository"/> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter.</param>
		public EmployeeRepository(IMapper mapper)
		{
			_context = new ApplicationContext();
			_mapper = mapper;
		}

		/// <summary>
		/// Creates new employee using underlying context.
		/// </summary>
		/// <param name="employee"><see cref="Employee"/> domain model to create.</param>
		public async Task<int> AddAsync(Employee employee)
		{
			if (employee.Id.GetValueOrDefault() > 0)
				throw new System.Exception();

			await _context.Employees.AddAsync(employee);
			await _context.SaveChangesAsync();

			return employee.Id.Value;
		}

		/// <summary>
		/// Gets employee and all associated vacations by given userId.
		/// User and Employee are 1:1 connection.
		/// </summary>
		/// <param name="id">Id parameter.</param>
		/// <returns><see cref="Employee"/> type with all his vacations.</returns>
		public async Task<Employee> GetAsync(int id)
		{
			return await _context.Employees.AsNoTracking().Include(x => x.Vacations).SingleOrDefaultAsync(x => x.Id == id);
		}

		/// <summary>
		/// Gets employee by userId.
		/// </summary>
		/// <param name="userId">UserId parameter.</param>
		/// <returns><see cref="Employee"/> type with all his vacations.</returns>
		public async Task<Employee> GetByUserAsync(int userId)
		{
			return await _context.Employees.AsNoTracking().Include(x => x.Vacations).SingleOrDefaultAsync(x => x.UserId == userId);
		}


		/// <summary>
		/// Gets all employees by page with their associated vacations from underlying context.
		/// </summary>
		/// <param name="page">page parameter.</param>
		/// <param name="resultsPerPage">Number of results per page.</param>
		/// <returns>Collection of <see cref="Employee"/> type with all their associated <see cref="Vacation"/></returns>
		public async Task<IEnumerable<Employee>> FindAsync(int page, int resultsPerPage)
		{
			int skip = resultsPerPage * (page - 1);
			return await _context.Employees.Include(x => x.Vacations).Skip(skip).Take(resultsPerPage).ToListAsync();
		}

		/// <summary>
		/// Updates employee.
		/// </summary>
		/// <param name="employee"><see cref="Employee"/> parameter.</param>
		/// <returns>True if successful.</returns>
		public async Task<bool> UpdateAsync(Employee employee)
		{
			if (employee.Id.GetValueOrDefault() == 0)
				return false;

			_context.Entry(employee).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return true;
		}

		/// <summary>
		/// Gets employee count.
		/// </summary>
		/// <returns>Employee count.</returns>
		public async Task<int> CountAsync()
		{
			return await _context.Employees.CountAsync();
		}
	}
}
