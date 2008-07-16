using NUnit.Framework;

namespace MyProject
{
	[TestFixture]
	public class SimpleClassTests
	{
		[Test]
		public void FailTest()
		{
			Assert.Fail("Test failed.");
		}

		[Test]
		public void PassTest()
		{
			Assert.IsTrue(true);
		}
	}
}