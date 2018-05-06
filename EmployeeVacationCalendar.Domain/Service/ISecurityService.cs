namespace EmployeeVacationCalendar.Domain.Service
{
	/// <summary>
	/// Security service interface.
	/// </summary>
	public interface ISecurityService
    {
		/// <summary>
		/// Encrypts string.
		/// </summary>
		/// <param name="data">string parameter.</param>
		/// <returns>encrypted string.</returns>
		string Encrypt(string data);

		/// <summary>
		/// Decrypts string.
		/// </summary>
		/// <param name="data">string paramter.</param>
		/// <returns>Decrypted string.</returns>
		string Decrypt(string data);

		/// <summary>
		/// Hashes string.
		/// </summary>
		/// <param name="data">string parameter.</param>
		/// <returns>Hashed string.</returns>
		string ComputeHash(string data);
    }
}
