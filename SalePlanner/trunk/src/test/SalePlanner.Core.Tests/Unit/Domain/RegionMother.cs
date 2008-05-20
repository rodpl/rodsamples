using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SalePlanner.Domain;

namespace SalePlanner.Core.Tests.Unit.Domain
{
	public static class RegionMother
	{
		public static Region CreateSimpleModel()
		{
			return new Region("Poland");
		}

		public static Region CreateSimpleTree()
		{
			var root = new Region("Poland");
			var north = new Region("North", root);
			var central = new Region("Central", root);
			var south = new Region("South", root);
			new Region("Gdansk", north);
			new Region("Szczecin", north);
			new Region("Warsaw", central);
			new Region("Poznan", central);
			new Region("Cracow", south);
			new Region("Wroc³aw", south);
			return root;
		}

		public static bool IsChildOf(this Region child, Region parent)
		{
			var result = true;
			Assert.That(child.Parent, Is.EqualTo(parent));
			Assert.That(child.IsRoot, Is.False);
			Assert.That(parent.Children, Has.Member(child));
			Assert.That(parent.IsLeaf, Is.False);
			return result;
		}
	}
}