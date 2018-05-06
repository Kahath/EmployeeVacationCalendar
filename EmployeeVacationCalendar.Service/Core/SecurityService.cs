using EmployeeVacationCalendar.Domain.Service;

namespace EmployeeVacationCalendar.Service.Core
{
	/// <summary>
	/// Data Encryption/Hashing.
	/// Not implemented so it returns plain string.
	/// </summary>
	public class SecurityService : ISecurityService
	{
		/// <summary>
		/// Hashes given string.
		/// Not implemented.
		/// </summary>
		/// <param name="data">data parameter.</param>
		/// <returns>Plain not hashed string.</returns>
		public string ComputeHash(string data)
		{
			return data;
		}

		/// <summary>
		/// Decrypts string.
		/// Not implemented.
		/// </summary>
		/// <param name="data">data parameter.</param>
		/// <returns>Plain not decrypted string.</returns>
		public string Decrypt(string data)
		{
			return data;
		}

		/// <summary>
		/// Encrypts string.
		/// Not implemented.
		/// </summary>
		/// <param name="data">data parameter.</param>
		/// <returns>Plain not encrypted string.</returns>
		public string Encrypt(string data)
		{
			return data;
		}
	}
}
