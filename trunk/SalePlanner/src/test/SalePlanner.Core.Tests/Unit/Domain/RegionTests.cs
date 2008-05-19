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
	}
}