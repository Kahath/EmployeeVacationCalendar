using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Web.Helpers;
using EmployeeVacationCalendar.Web.ViewModels.Calendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Web.Controllers
{
	/// <summary>
	/// Employee data manipulation.
	/// </summary>
	public class EmployeeController : Controller
    {
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// <see cref="EmployeeController"/> constructor.
		/// </summary>
		/// <param name="mapper"><see cref="IMapper"/> parameter.</param>
		/// <param name="employeeRepository"><see cref="IEmployeeRepository"/> parameter.</param>
		public EmployeeController(IMapper mapper, IEmployeeRepository employeeRepository)
		{
			_mapper = mapper;
			_employeeRepository = employeeRepository;
		}

		/// <summary>
		/// Gets employee by id.
		/// </summary>
		/// <param name="id">Id parameter.</param>
		/// <returns>Json employee data if found.</returns>
		[Route("[controller]/[action]/{id}", Name = "GetEmployee")]
		[HttpGet]
		public async Task<IActionResult> Get(int id)
        {
			if (id == 0)
				return BadRequest("Id must not be empty");

			Employee existingEmployee = await _employeeRepository.GetAsync(id);
			if (existingEmployee == null)
				return NotFound("Employee does not exist");

			int userId = existingEmployee.UserId;
			int authenticatedUserId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);
			bool isAdmin = User.IsInRole(Roles.Admin);

			if (!isAdmin && userId != authenticatedUserId)
				return Unauthorized();

			CalendarEmployeeViewModel viewModel = _mapper.Map<CalendarEmployeeViewModel>(existingEmployee);

			return Json(viewModel);
        }

		/// <summary>
		/// Updates employee by given viewModel.
		/// </summary>
		/// <param name="viewModel"><see cref="CalendarEmployeeViewModel"/> parameter.</param>
		/// <returns>NoContent (204) status code if successful.</returns>
		[Route("[controller]/[action]/")]
		[HttpPut]
		public async Task<IActionResult> Update(CalendarEmployeeViewModel viewModel)
		{
			if (viewModel.Id == 0)
				return BadRequest("Employee id must not be empty");

			Employee existingEmployee = await _employeeRepository.GetAsync(viewModel.Id);
			if (existingEmployee == null)
				return NotFound("Employee does not exist");

			int userId = existingEmployee.UserId;
			int authenticatedUserId = Int32.Parse(User.Claims.Single(x => x.Type == ApplicationClaimTypes.UserId).Value);
			bool isAdmin = User.IsInRole(Roles.Admin);

			Employee employee = new Employee()
			{
				Id = existingEmployee.Id,
				FirstName = viewModel.FirstName,
				LastName = viewModel.LastName,
				UserId = existingEmployee.UserId
			};

			if (!await _employeeRepository.UpdateAsync(employee))
				return StatusCode(StatusCodes.Status500InternalServerError);

			return NoContent();
		}
	}
}