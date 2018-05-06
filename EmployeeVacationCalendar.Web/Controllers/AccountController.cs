using AutoMapper;
using EmployeeVacationCalendar.Domain.Model;
using EmployeeVacationCalendar.Domain.Repository;
using EmployeeVacationCalendar.Domain.Service;
using EmployeeVacationCalendar.Web.Globalization;
using EmployeeVacationCalendar.Web.Helpers;
using EmployeeVacationCalendar.Web.Models;
using EmployeeVacationCalendar.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeVacationCalendar.Web.Controllers
{
	/// <summary>
	/// Controller for authentication and registration.
	/// </summary>
	public class AccountController : Controller
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly ISecurityService _securityService;

		/// <summary>
		/// <see cref="AccountController"/> constructor.
		/// </summary>
		/// <param name="mapper">Injected <see cref="IMapper"/> parameter.</param>
		/// <param name="userService">Injected <see cref="IUserService"/> parameter.</param>
		/// <param name="config">Injected <see cref="IConfiguration"/> parameter.</param>
		/// <param name="userRepository">Injected <see cref="IUserRepository"/> parameter.</param>
		/// <param name="securityService">Injected <see cref="ISecurityService"/> parameter.</param>
		public AccountController(IMapper mapper, IUserService userService, IConfiguration config, IUserRepository userRepository, ISecurityService securityService)
		{
			_mapper = mapper;
			_userService = userService;
			_userRepository = userRepository;
			_securityService = securityService;
		}

		/// <summary>
		/// Login view.
		/// </summary>
		/// <param name="returnUrl">redirect url after successful login.</param>
		/// <returns>Login view.</returns>
		public IActionResult Login(string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
				return Redirect("/");

			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		/// <summary>
		/// Attempts to authenticate user by given viewModel.
		/// </summary>
		/// <param name="viewModel">ViewModel parameter.</param>
		/// <param name="returnUrl">redirect url after successful login.</param>
		/// <returns>Redirects to returnUrl parameter or default route if no url is supplied.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl)
		{
			if (!ModelState.IsValid)
				return View();

			User user = await _userRepository.GetByUserNameAsync(viewModel.Username);

			if (user == null)
			{
				ModelState.AddModelError(nameof(ErrorCode.UsernameNotFound), Resources.UserNameNotFound);
				return View();
			}

			if (user.Password != _securityService.ComputeHash(viewModel.Password))
			{
				ModelState.AddModelError(nameof(ErrorCode.InvalidPassword), Resources.WrongPassword);
				return View();
			}

			IEnumerable<Role> userRoles = await _userRepository.GetUserRolesAsync(user.Id.GetValueOrDefault());

			List<Claim> claims = new List<Claim>() {
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.UserName),
				new Claim(ApplicationClaimTypes.UserId,  user.Id.ToString()),
			};

			foreach (Role role in userRoles)
				claims.Add(new Claim(ClaimTypes.Role, role.Name));

			ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsPrincipal principal = new ClaimsPrincipal(identity);

			await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTime.Now.AddMinutes(30).ToUniversalTime() });

			if (Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);

			return Redirect("/");
		}

		/// <summary>
		/// Attempts to sign out user.
		/// </summary>
		/// <returns>Default route view.</returns>
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
				await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);

			return Redirect("/");
		}

		/// <summary>
		/// Register view.
		/// </summary>
		/// <returns>Register view.</returns>
		[Authorize(Roles = Roles.Admin)]
		public IActionResult Register()
		{
			return View();
		}

		/// <summary>
		/// Attempts to register user by given viewModel.
		/// </summary>
		/// <param name="viewModel">ViewModel parameter.</param>
		/// <returns>Redirects to default route.</returns>
		[HttpPost]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> Register(RegisterViewModel viewModel)
		{
			if (!ModelState.IsValid)
				return View();

			User user = await _userRepository.GetByUserNameAsync(viewModel.Username);

			if(user != null)
			{
				ModelState.AddModelError(nameof(ErrorCode.UsernameAlreadyExists), Resources.UsernameAlreadyExists);
				return View();
			}

			string passwordHash = _securityService.ComputeHash(viewModel.Password);
			User newUser = new User()
			{
				UserName = viewModel.Username,
				Password = passwordHash,
			};

			Employee employee = new Employee()
			{
				FirstName = viewModel.FirstName,
				LastName = viewModel.LastName,
			};

			if(!await _userService.AddAsync(newUser, employee))
			{
				ModelState.AddModelError(nameof(ErrorCode.GenericError), Resources.GenericError);
				return View();
			}

			return Redirect("/");
		}
	}
}