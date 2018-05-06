using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Repository.Core
{
	/// <summary>
	/// Vacation data manipulation.
	/// </summary>
	public class VacationRepository : IVacationRepository
	{
		private readonly ApplicationContext _context;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="VacationRepository"/> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter.</param>
		public VacationRepository(IMapper mapper)
		{
			_context = new ApplicationContext();
			_mapper = mapper;
		}

		/// <summary>
		/// Creates new vacation in underlying context.
		/// </summary>
		/// <param name="vacation"><see cref="Vacation"/> parameter.</param>
		/// <returns>Added vacation identity.</returns>
		public async Task<int> AddAsync(Vacation vacation)
		{
			if (vacation.Id.GetValueOrDefault() > 0)
				throw new System.Exception();

			await _context.Vacations.AddAsync(vacation);
			await _context.SaveChangesAsync();

			return vacation.Id.Value;
		}

		/// <summary>
		/// Updates existing vacation in underlying context.
		/// </summary>
		/// <param name="vacation"><see cref="Vacation"/> parameter.</param>
		/// <returns>True if successful.</returns>
		public async Task<bool> UpdateAsync(Vacation vacation)
		{
			if (vacation.Id.GetValueOrDefault() == 0)
				return false;

			_context.Entry(vacation).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return true;
		}

		/// <summary>
		/// Gets vacation by given Id.
		/// </summary>
		/// <param name="id">Id parameter.</param>
		/// <returns><see cref="Vacation"/> object.</returns>
		public async Task<Vacation> GetAsync(int id)
		{
			return await _context.Vacations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

		}

		/// <summary>
		/// Deletes vacation from underlying context by given Id.
		/// </summary>
		/// <param name="id">Id parameter.</param>
		public async Task<bool> RemoveAsync(int id)
		{
			Vacation vacation = await _context.Vacations.SingleOrDefaultAsync(x => x.Id == id);

			if (vacation == null)
				return false;

			_context.Vacations.Remove(vacation);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
