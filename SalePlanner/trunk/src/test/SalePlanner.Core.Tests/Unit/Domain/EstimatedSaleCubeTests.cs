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
		public void Ctor_AllDimensionsHasOnlyOneLevel_AllocatedAmountEqualsToAmount()
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

		[Test]
		public void Ctor_AtLeastOneDimensionHasMoreThanOneLevel_FreeAmountEqualsToAmount()
		{
			// One level dimension means that root is leaf.
			// Region will have more than one level and Period will have one level.
			var amount = 1000m;
			var regionRoot = new Region("far far away");
			new Region("northern lands", regionRoot);
			new Region("southern lands", regionRoot);
			var period = new Period("long time ago");
			var model = new EstimatedSaleCube(amount, regionRoot, period);

			// Check if root is not leaf
			Assert.That(model.RegionRoot.IsRoot && (model.RegionRoot.IsLeaf == false));
			// Check if root is leafs
			Assert.That(model.PeriodRoot.IsRoot && model.PeriodRoot.IsLeaf);

			Assert.That(model.Amount, Is.EqualTo(amount));
			Assert.That(model.AllocatedAmount, Is.EqualTo(0));
			Assert.That(model.FreeAmount, Is.EqualTo(amount));
		}

		[Test]
		public void FindFor_CriteriesForNonExistingYetAllocation_ReturnsNullAllocationObject()
		{
			var model = EstimatedSaleCubeMother.CreateSimpleModel();

			var regionWarsaw = model.RegionRoot.Find(r => r.Name.Equals("Warsaw"));
			Assert.That(regionWarsaw, Is.Not.Null);
			var periodRoot = model.PeriodRoot;

			var allocation = model.FindFor(regionWarsaw, periodRoot);
			Assert.That(allocation, Is.EqualTo(EstimatedSaleAllocation.NULL));
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

			Assert.That(model.Amount, Is.EqualTo(amount));
			Assert.That(model.RegionDimension, Is.EqualTo(region));
			Assert.That(model.PeriodDimension, Is.EqualTo(period));
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