using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Web.Helpers;
using EmployeeVacationCalendar.Web.ViewModels.Calendar;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Web.Controllers
{
	/// <summary>
	/// Provides calendar per year/month.
	/// </summary>
	public class CalendarController : Controller
	{
		private readonly ICalendarRepository _calendarRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IVacationRepository _vacationRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="CalendarController" /> constructor.
		/// </summary>
		/// <param name="calendarRepository">Injected <see cref="ICalendarRepository"/> parameter.</param>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter.</param>
		/// <param name="employeeRepository">Injected <see cref="IEmployeeRepository"/> parameter.</param>
		/// <param name="vacationRepository">Injected <see cref="IVacationRepository"/> parameter.</param>
		public CalendarController(ICalendarRepository calendarRepository, IMapper mapper, IEmployeeRepository employeeRepository, IVacationRepository vacationRepository)
		{
			_mapper = mapper;
			_calendarRepository = calendarRepository;
			_employeeRepository = employeeRepository;
			_vacationRepository = vacationRepository;
		}

		/// <summary>
		/// Gets calendar view.
		/// </summary>
		/// <returns>Calendar view.</returns>
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Returns json of all calendar days (weekend, vacation, holiday) for all users by given year, month and pa ge.
		/// Data is composed in employee -> months -> weeks -> days format.
		/// If no parameters are passed current datetime year and month are used.
		/// This is called from react EmployeesCalendar component.
		/// </summary>
		/// <param name="year">Calendar year.</param>
		/// <param name="month">Calendar month.</param>
		/// <param name="page">Page parameter.</param>
		/// <returns>Json calendar data.</returns>
		[HttpGet]
		[Route("[controller]/[action]/{year}/{month}/{page}")]
		public async Task<IActionResult> GetCalendar(int year, int month, int page)
		{
			year = year == 0 ? DateTime.Now.Year : year;
			month = month == 0 ? DateTime.Now.Month : month;
			page = page == 0 ? 1 : page;

			int resultsPerPage = 3;
			int employeeCount = await _employeeRepository.CountAsync();
			int maxPages = (int)Math.Ceiling(((double)employeeCount) / resultsPerPage);

			IEnumerable<CalendarDayViewModel> calendarDays = _mapper.Map<IEnumerable<CalendarDayViewModel>>(await _calendarRepository.FindAsync(year, month));
			List<CalendarEmployeeViewModel> employees = _mapper.Map<IEnumerable<CalendarEmployeeViewModel>>(await _employeeRepository.FindAsync(page, resultsPerPage)).ToList();

			int userId = 0;
			if (User.Identity.IsAuthenticated)
				userId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);

			bool isAdmin = User.IsInRole(Roles.Admin);

			CalendarViewModel retVal = new CalendarViewModel(employees, calendarDays, userId, isAdmin, maxPages);

			return Json(retVal);
		}
	}
}