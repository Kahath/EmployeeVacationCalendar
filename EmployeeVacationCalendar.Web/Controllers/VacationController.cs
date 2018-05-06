using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Web.Helpers;
using EmployeeVacationCalendar.Web.ViewModels.Calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Web.Controllers
{
	/// <summary>
	/// Provides vacation data manipulation
	/// </summary>
	[Authorize]
	public class VacationController : Controller
    {
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IVacationRepository _vacationRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="VacationController" /> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter.</param>
		/// <param name="employeeRepository">Injected <see cref="IEmployeeRepository"/> parameter.</param>
		/// <param name="vacationRepository">Injected <see cref="IVacationRepository"/> parameter.</param>
		public VacationController(IMapper mapper, IEmployeeRepository employeeRepository, IVacationRepository vacationRepository)
		{
			_mapper = mapper;
			_employeeRepository = employeeRepository;
			_vacationRepository = vacationRepository;
		}


		/// <summary>
		/// Gets vacation data from repository by given id.
		/// this is used when user pops up dialog by clicking on calendar day.
		/// </summary>
		/// <param name="id">VacationId parameter.</param>
		/// <returns>Json vacation data.</returns>
		[Route("[controller]/[action]/{id}", Name = "GetVacation")]
		[HttpGet]
		public async Task<IActionResult> Get(int id)
		{
			if (id == 0)
				return BadRequest("Id can not be empty.");

			Vacation vacation = await _vacationRepository.GetAsync(id);
			if (vacation == null)
				return NotFound("Vacation not found.");

			CalendarVacationViewModel vacationViewModel = _mapper.Map<CalendarVacationViewModel>(vacation);

			return Json(vacation);
		}

		/// <summary>
		/// Adds new entity to repository by given viewModel.
		/// Users can add only their vacations, but Admin role can add all.
		/// </summary>
		/// <param name="vacationViewModel">ViewModel parameter.</param>
		/// <returns>Json data. True for success, error string for failure.</returns>
		[Route("[controller]/[action]/")]
		[HttpPost]
		public async Task<IActionResult> Add(CalendarVacationViewModel vacationViewModel)
		{
			if (vacationViewModel.Id.GetValueOrDefault() != 0)
				return BadRequest("Id must be empty.");

			if (vacationViewModel.Type == VacationType.None)
				return BadRequest("Vacation type can not be empty.");

			if (vacationViewModel.DateFrom > vacationViewModel.DateTo)
				return BadRequest("Date from can not be greater than date to.");

			Employee employee = await _employeeRepository.GetAsync(vacationViewModel.EmployeeId);
			if (employee == null)
				return NotFound("Employee does not exist.");

			int userId = employee.UserId;
			int authenticatedUserId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);
			bool isAdmin = User.IsInRole(Roles.Admin);

			if (!isAdmin && userId != authenticatedUserId)
				return Unauthorized();

			if (employee.Vacations.Any(x => x.DateFrom <= vacationViewModel.DateTo && vacationViewModel.DateFrom <= x.DateTo && x.Id != vacationViewModel.Id))
				return BadRequest("Date range intersects with other vacation.");

			Vacation vacation = new Vacation()
			{
				EmployeeId = employee.Id.Value,
				DateFrom = vacationViewModel.DateFrom,
				DateTo = vacationViewModel.DateTo,
				Type = vacationViewModel.Type
			};

			vacation.Id = await _vacationRepository.AddAsync(vacation);

			return CreatedAtRoute("GetVacation", new { id = vacation.Id }, vacation);
		}

		/// <summary>
		/// Updates existing vacation in repository by given viewModel.
		/// Users can update only their vacations, but Admin role can update all.
		/// </summary>
		/// <param name="vacationViewModel">ViewModel parameter.</param>
		/// <returns>Json data. True for success, error string for failure.</returns>
		[Route("[controller]/[action]/")]
		[HttpPut]
		public async Task<IActionResult> Update(CalendarVacationViewModel vacationViewModel)
		{
			if (vacationViewModel.Id == 0)
				return BadRequest("Id can not be null.");

			if (vacationViewModel.Type == VacationType.None)
				return BadRequest("Vacation type can not be empty.");

			if (vacationViewModel.DateFrom > vacationViewModel.DateTo)
				return BadRequest("Date from can not be greater than date to.");

			Employee employee = await _employeeRepository.GetAsync(vacationViewModel.EmployeeId);
			if (employee == null)
				return NotFound("Employee does not exist.");

			int userId = employee.UserId;
			int authenticatedUserId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);
			bool isAdmin = User.IsInRole(Roles.Admin);

			if (!isAdmin && userId != authenticatedUserId)
				return Unauthorized();

			Vacation existingVacation = await _vacationRepository.GetAsync(vacationViewModel.Id.Value);
			if (existingVacation == null)
				return NotFound("Vacation entry does not exist.");

			if (employee.Vacations.Any(x => x.DateFrom <= vacationViewModel.DateTo && vacationViewModel.DateFrom <= x.DateTo && x.Id != vacationViewModel.Id))
				return BadRequest("Date range intersects with other vacation.");

			Vacation vacation = new Vacation()
			{
				Id = vacationViewModel.Id.Value,
				EmployeeId = employee.Id.Value,
				DateFrom = vacationViewModel.DateFrom,
				DateTo = vacationViewModel.DateTo,
				Type = vacationViewModel.Type
			};

			if (!await _vacationRepository.UpdateAsync(vacation))
				return StatusCode(StatusCodes.Status500InternalServerError);

			return NoContent();
		}

		/// <summary>
		/// Deletes vacation from repository by given viewModel.
		/// Users can delete only their vacations, but Admin role can update all.
		/// </summary>
		/// <param name="id">Vacation id.</param>
		/// <returns>Json data. True for success, error string for failure.</returns>
		[Route("[controller]/[action]/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			if (id == 0)
				return BadRequest("Id can not be null.");

			Vacation existingVacation = await _vacationRepository.GetAsync(id);
			if (existingVacation == null)
				return NotFound("Vacation entry does not exist.");

			Employee employee = await _employeeRepository.GetAsync(existingVacation.EmployeeId);
			if (employee == null)
				return BadRequest("Employee does not exist.");

			int userId = employee.UserId;
			int authenticatedUserId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);
			bool isAdmin = User.IsInRole(Roles.Admin);

			if (!isAdmin && userId != authenticatedUserId)
				return Unauthorized();

			if (!await _vacationRepository.RemoveAsync(id))
				return StatusCode(StatusCodes.Status500InternalServerError);

			return NoContent();
		}
	}
}