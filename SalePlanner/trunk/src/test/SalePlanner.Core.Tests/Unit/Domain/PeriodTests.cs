using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SalePlanner.Domain;

namespace SalePlanner.Core.Tests.Unit.Domain
{
	[TestFixture]
	public class PeriodTests
	{
		[Test]
		public void Ctor_WithNameOnly_SetsNameAndMakeNodeAsRootAndAsLeaf_Test()
		{
			var name = "2008";
			var model = new Period(name);

			Assert.That(model.Name, Is.EqualTo(name));
			Assert.That(model.IsRoot);
			Assert.That(model.IsLeaf);
		}

		[Test]
		public void Ctor_WithNameAndParent_SetsNameAndMakeNodeAsChild_Test()
		{
			var root = new Period("2008");

			var name = "1st Half";
			var model = new Period(name, root);

			Assert.That(model.Name, Is.EqualTo(name));
			Assert.That(model.IsLeaf);

			Assert.That(model.IsChildOf(root));
		}
	}
}