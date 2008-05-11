using System.Collections.Generic;
using SalePlanner.Domain;

namespace SalePlanner.DataAccess
{
	/// <summary>
	/// Repository for period object as hierarchies.
	/// </summary>
	public interface PeriodRepository : Repository
	{
		/// <summary>
		/// Gets all roots for period hierarchies.
		/// </summary>
		/// <returns>List of period hierarchies.</returns>
		/// <remarks>Period can have many roots for particular years like 2006 and 2007 and so on.</remarks>
		ICollection<Period> GetAllRoots();
	}
}