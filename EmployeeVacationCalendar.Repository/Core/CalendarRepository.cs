using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Repository.Core
{
	/// <summary>
	/// Read only calendar repository.
	/// Provides collection of days.
	/// </summary>
	public class CalendarRepository : ICalendarRepository
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="CalendarRepository"/> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> property.</param>
		public CalendarRepository(IMapper mapper)
		{
			_context = new ApplicationContext();
			_mapper = mapper;
		}

		/// <summary>
		/// Gets collection of days by given year and month.
		/// if month is 0 it is not passed as parameter to underlying context.
		/// </summary>
		/// <param name="year">Calendar year.</param>
		/// <param name="month">Calendar month.</param>
		/// <returns>Collection of calendar days.</returns>
		public async Task<IEnumerable<CalendarDay>> FindAsync(int year, int month)
		{
			return await _context.Calendar.AsNoTracking().Where(x => x.Year == year && x.Month == month).ToListAsync();
		}
	}
}
