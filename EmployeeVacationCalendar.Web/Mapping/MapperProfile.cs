using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Web.ViewModels.Calendar;

namespace EmployeeVacationCalendar.Web.Mapping
{
	/// <summary>
	/// Automapper profile for web project.
	/// Binding domain models to viewModels.
	/// </summary>
	public class MapperProfile : Profile
	{
		/// <summary>
		/// <see cref="MapperProfile"/> constructor.
		/// </summary>
		public MapperProfile()
		{
			CreateMap<Employee, CalendarEmployeeViewModel>();
			CreateMap<CalendarDay, CalendarDayViewModel>();
			CreateMap<Vacation, CalendarVacationViewModel>();
		}
	}
}
