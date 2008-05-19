using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace SalePlanner.Domain
{
	/// <summary>
	/// Class representes single sale estimation plan.
	/// </summary>
	public class EstimatedSaleCube
	{
		/// <summary>
		/// Gets the total amount wchich is available to allocate.
		/// </summary>
		/// <value>The amount to allocate.</value>
		public decimal Amount { get; private set; }

		/// <summary>
		/// Gets the totally allocated amount in the cube.
		/// </summary>
		/// <value>The allocated amount.</value>
		public decimal AllocatedAmount
		{
			get
			{
				return (from dim in (Allocations as List<EstimatedSaleAllocation>)
						where dim.RegionDimension.IsLeaf
						select (dim.AllocatedAmount)).Sum();
			}
		}

		/// <summary>
		/// Gets the free amount, not allocated in cube.
		/// </summary>
		/// <value>The free amount.</value>
		public decimal FreeAmount
		{
			get
			{
				return 0m;
			}
		}

		/// <summary>
		/// Gets the root of region dimension.
		/// </summary>
		/// <value>The region root.</value>
		public Region RegionRoot { get; private set; }

		/// <summary>
		/// Gets the root of period dimension.
		/// </summary>
		/// <value>The period root.</value>
		public Period PeriodRoot { get; private set; }

		/// <summary>
		/// Gets or sets the list of allocations.
		/// </summary>
		/// <value>The allocations.</value>
		private IList<EstimatedSaleAllocation> Allocations { get; set; }

		#region contructors...

		/// <summary>
		/// Initializes a new instance of the <see cref="EstimatedSaleCube"/> class.
		/// </summary>
		/// <param name="amount">The initial amount.</param>
		/// <param name="regionRoot">The root of region dimension.</param>
		/// <param name="periodRoot">The root of period dimension.</param>
		public EstimatedSaleCube(decimal amount, Region regionRoot, Period periodRoot)
		{
			if (amount <= 0) throw new ArgumentOutOfRangeException("amount");
			if (regionRoot == null) throw new ArgumentNullException("regionRoot");
			if (periodRoot == null) throw new ArgumentNullException("periodRoot");

			Amount = amount;
			RegionRoot = regionRoot;
			PeriodRoot = periodRoot;

			Allocations = new List<EstimatedSaleAllocation> { new EstimatedSaleAllocation(amount, regionRoot, periodRoot) };
		}

		#endregion
	}

	/// <summary>
	/// Class represents single amount allocation in sale planning.
	/// </summary>
	public class EstimatedSaleAllocation
	{
		public decimal AllocatedAmount { get; set; }
		public decimal FreeAmount { get; set; }

		// Dimensions
		public Region RegionDimension { get; set; }
		public Period PeriodDimension { get; set; }

		#region contructors...

		public EstimatedSaleAllocation(decimal amount, Region region, Period period)
		{
			if (region == null) throw new ArgumentNullException("region");
			if (period == null) throw new ArgumentNullException("period");

			AllocatedAmount = amount;
			RegionDimension = region;
			PeriodDimension = period;
		}

		#endregion

		public void ReleaseAllocatedAmount(decimal amountToRelease)
		{
			if (amountToRelease < 0 || amountToRelease > AllocatedAmount) throw new ArgumentOutOfRangeException("amountToRelease");
			AllocatedAmount -= amountToRelease;
		}

		public void ReleaseAllAllocatedAmount()
		{
			AllocatedAmount = 0;
		}
	}
}