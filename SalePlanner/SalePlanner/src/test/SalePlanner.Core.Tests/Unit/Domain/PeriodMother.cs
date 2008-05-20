using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SalePlanner.Domain;

namespace SalePlanner.Core.Tests.Unit.Domain
{
	public static class PeriodMother
	{
		public static Period CreateSimpleModel()
		{
			return new Period("Year 2008");
		}

		public static Period CreateSimpleTree()
		{
			var root = new Period("2008");
			var firstQuarter = new Period("1st Quarter", root);
			var secondQuarter = new Period("2nd Quarter", root);
			var thirdQuarter = new Period("3rd Quarter", root);
			var fourthQuarter = new Period("4th Quarter", root);
			new Period("January", firstQuarter);
			new Period("February", firstQuarter);
			new Period("March", firstQuarter);
			new Period("April", secondQuarter);
			new Period("May", secondQuarter);
			new Period("June", secondQuarter);
			new Period("July", thirdQuarter);
			new Period("August", thirdQuarter);
			new Period("September", thirdQuarter);
			new Period("October", fourthQuarter);
			new Period("November", fourthQuarter);
			new Period("December", fourthQuarter);
			return root;
		}

		public static bool IsChildOf(this Period child, Period parent)
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