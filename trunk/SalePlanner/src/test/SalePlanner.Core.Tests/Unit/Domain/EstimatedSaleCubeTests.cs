using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SalePlanner.Domain;

namespace SalePlanner.Core.Tests.Unit.Domain
{
	[TestFixture]
	public class EstimatedSaleCubeTests
	{
		[Test]
		public void Ctor_WithValidArguments_CreatesModel()
		{
			var amount = 1000m;
			var regionRoot = RegionMother.CreateSimpleTree();
			var periodRoot = PeriodMother.CreateSimpleTree();
			var model = new EstimatedSaleCube(amount, regionRoot, periodRoot);

			Assert.That(model.Amount, Is.EqualTo(amount));
			Assert.That(model.RegionRoot, Is.EqualTo(regionRoot));
			Assert.That(model.PeriodRoot, Is.EqualTo(periodRoot));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Ctor_WithZeroAmount_ThrowsArgumentOutOfRangeException()
		{
			var amount = 0m;
			var regionRoot = RegionMother.CreateSimpleTree();
			var periodRoot = PeriodMother.CreateSimpleTree();
			new EstimatedSaleCube(amount, regionRoot, periodRoot);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Ctor_WithNegativeAmount_ThrowsArgumentOutOfRangeException()
		{
			var amount = -100m;
			var regionRoot = RegionMother.CreateSimpleTree();
			var periodRoot = PeriodMother.CreateSimpleTree();
			new EstimatedSaleCube(amount, regionRoot, periodRoot);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_WithNullRegionRoot_ThrowsArgumentNullException()
		{
			var amount = 1000m;
			var periodRoot = PeriodMother.CreateSimpleTree();
			new EstimatedSaleCube(amount, null, periodRoot);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_WithNullPeriodRoot_ThrowsArgumentNullException()
		{
			var amount = 1000m;
			var regionRoot = RegionMother.CreateSimpleTree();
			new EstimatedSaleCube(amount, regionRoot, null);
		}

		[Test]
		public void Ctor_ForOnlyOneLevelDimension_AllocatesAllAmount()
		{
			// One level dimension means that root is leaf.
			var amount = 1000m;
			var region = new Region("far far away");
			var period = new Period("long time ago");
			var model = new EstimatedSaleCube(amount, region, period);

			// Check if roots are leafs
			Assert.That(model.RegionRoot.IsRoot && model.RegionRoot.IsLeaf);
			Assert.That(model.PeriodRoot.IsRoot && model.PeriodRoot.IsLeaf);

			Assert.That(model.Amount, Is.EqualTo(amount));
			Assert.That(model.AllocatedAmount, Is.EqualTo(amount));
			Assert.That(model.FreeAmount, Is.EqualTo(0));
		}
	}

	public static class EstimatedSaleCubeMother
	{
		public static EstimatedSaleCube CreateSimpleModel()
		{
			return new EstimatedSaleCube(1000m, RegionMother.CreateSimpleTree(), PeriodMother.CreateSimpleTree());
		}
	}

	[TestFixture]
	public class EstimatedSaleAllocationTests
	{
		[Test]
		public void Ctor_WithValidArguments_Test()
		{
			var amount = 1000m;
			var region = RegionMother.CreateSimpleModel();
			var period = PeriodMother.CreateSimpleModel();
			var model = new EstimatedSaleAllocation(amount, region, period);

			Assert.That(model.AllocatedAmount, Is.EqualTo(amount));
			Assert.That(model.RegionDimension, Is.EqualTo(region));
			Assert.That(model.PeriodDimension, Is.EqualTo(period));
		}

		[Test]
		public void ReleaseAllAllocatedAmount_MakesAmountEqualToZero_Test()
		{
			var model = EstimatedSaleAllocationMother.CreateSimpleModel();

			model.ReleaseAllAllocatedAmount();
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReleaseAllocatedAmount_ArgumentLessThanZero_ThrowsArgumentOutOfRangeException_Test()
		{
			var model = EstimatedSaleAllocationMother.CreateSimpleModel();
			var amountToRelease = -1m;

			model.ReleaseAllocatedAmount(amountToRelease);
		}

		[Test]
		public void ReleaseAllocatedAmount_CurrentAmountIsGraterThanArgument_DecreasesAmount_Test()
		{
			var model = EstimatedSaleAllocationMother.CreateSimpleModel();
			var initialAmount = model.AllocatedAmount;
			var amountToRelease = 500m;

			Assert.That(initialAmount, Is.GreaterThan(amountToRelease));

			model.ReleaseAllocatedAmount(amountToRelease);

			Assert.That(model.AllocatedAmount, Is.EqualTo(initialAmount - amountToRelease));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReleaseAllocatedAmount_CurrentAmountIsLessThanArgument_ThrowsArgumentOutOfRangeException_Test()
		{
			var model = EstimatedSaleAllocationMother.CreateSimpleModel();
			var initialAmount = model.AllocatedAmount;
			var amountToRelease = 1500m;

			Assert.That(initialAmount, Is.LessThan(amountToRelease));

			model.ReleaseAllocatedAmount(amountToRelease);
		}
	}

	public static class EstimatedSaleAllocationMother
	{
		public static EstimatedSaleAllocation CreateSimpleModel()
		{
			var amount = 1000m;
			return new EstimatedSaleAllocation(amount, RegionMother.CreateSimpleModel(), PeriodMother.CreateSimpleModel());
		}
	}
}