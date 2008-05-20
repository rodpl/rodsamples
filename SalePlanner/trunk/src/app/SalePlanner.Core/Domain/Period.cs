using System.Collections.Generic;

namespace SalePlanner.Domain
{
	/// <summary>
	/// Class represents single period in period hierarchy.
	/// It will be used as one of dimension for sale planning.
	/// <remarks>Usually period should has date range but for this example I don't need that.</remarks>
	/// </summary>
	public class Period : Hierarchical
	{
		public string Name { get; set; }
		public Period Parent { get; set; }

		public IList<Period> Children
		{
			get { return _children.AsReadOnly(); }
		}

		public bool IsRoot
		{
			get { return Parent == null; }
		}

		public bool IsLeaf
		{
			get { return _children.Count == 0; }
		}

		private readonly List<Period> _children = new List<Period>();

		#region contructors...

		public Period(string name)
		{
			Name = name;
		}

		public Period(string name, Period parent)
			: this(name)
		{
			Parent = parent;
			Parent.AddChild(this);
		}

		#endregion

		public void AddChild(Period child)
		{
			if (!_children.Contains(child))
				_children.Add(child);
			if (child.Parent != this)
				child.Parent = this;
		}
	}
}