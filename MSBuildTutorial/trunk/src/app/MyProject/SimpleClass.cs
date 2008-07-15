namespace MyProject
{
	public class SimpleClass
	{
		public string Name { get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the SimpleClass class.
		/// </summary>
		public SimpleClass(string name)
		{
			Name = name;
		}
	}
}
