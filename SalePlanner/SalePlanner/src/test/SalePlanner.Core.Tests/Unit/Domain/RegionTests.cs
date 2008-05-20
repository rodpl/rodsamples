using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SalePlanner.Domain;

namespace SalePlanner.Core.Tests.Unit.Domain
{
	[TestFixture]
	public class RegionTests
	{
		[Test]
		public void Ctor_WithNameOnly_SetsNameAndMakeNodeAsRootAndAsLeaf_Test()
		{
			var name = "Poland";
			var model = new Region(name);

			Assert.That(model.Name, Is.EqualTo(name));
			Assert.That(model.IsRoot);
			Assert.That(model.IsLeaf);
		}

		[Test]
		public void Ctor_WithNameAndParent_SetsNameAndMakeNodeAsChild_Test()
		{
			var root = new Region("Poland");

			var name = "North";
			var model = new Region(name, root);

			Assert.That(model.Name, Is.EqualTo(name));
			Assert.That(model.IsLeaf);

			Assert.That(model.IsChildOf(root));
		}

		[Test]
		public void GetEnumerator_SingleNodeRoot_ReturnsItself()
		{
			var root = new Region("Poland");
			var counter = 0;
			Region child = null;
			foreach(var item in root)
			{
				child = item;
				counter++;
			}

			Assert.That(counter, Is.EqualTo(1));
			Assert.That(child, Is.EqualTo(root));
		}

		[Test]
		public void GetEnumerator_SimpleTree_Returns10Nodes()
		{
			var root = RegionMother.CreateSimpleTree();
			var counter = 0;
			foreach (var item in root)
			{
				counter++;
			}

			Assert.That(counter, Is.EqualTo(10));
		}

		[Test]
		public void FindAll_NonExistingCriteria_ReturnsNull()
		{
			var model = RegionMother.CreateSimpleTree();
			var result = model.FindAll(region => region.Name.Equals("Non existing"));

			Assert.That(result, Is.Null);
		}

		[Test]
		public void FindAll_ExistingCriteria_ReturnsList()
		{
			var model = RegionMother.CreateSimpleTree();
			var result = model.FindAll(region => region.IsLeaf);

			Assert.That(result, Is.Not.Null);
			var counter = 0;
			foreach(var item in result)
			{
				Assert.That(item.IsLeaf);
				counter++;
			}
			// There is 6 cities
			Assert.That(counter, Is.EqualTo(6));
		}

		[Test] public void Find_NonExistingCriteria_ReturnsNull()
		{
			var model = RegionMother.CreateSimpleTree();
			var result = model.Find(region => region.Name.Equals("Non existing"));

			Assert.That(result, Is.Null);
		}

		[Test]
		public void Find_ExistingCriteria_ReturnsFirstSingleRegionMeetingCriteria()
		{
			var model = RegionMother.CreateSimpleTree();
			var result = model.Find(region => region.Name.Equals("Warsaw"));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Name, Is.EqualTo("Warsaw"));
		}
	}
}