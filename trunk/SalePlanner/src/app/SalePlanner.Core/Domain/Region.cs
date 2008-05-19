using System.Collections.Generic;

namespace SalePlanner.Domain
{
	/// <summary>
	/// Class represents single territorial region in hierarchy.
	/// It will be used as one of dimension for sale planning.
	/// </summary>
	public class Region
	{
		public string Name { get; set; }
		public Region Parent { get; set; }

		public IList<Region> Children
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

		private readonly List<Region> _children = new List<Region>();

		#region contructors...

		public Region(string name)
		{
			Name = name;
		}

		public Region(string name, Region parent)
			: this(name)
		{
			Parent = parent;
			Parent.AddChild(this);
		}

		#endregion

		public void AddChild(Region child)
		{
			if (!_children.Contains(child))
				_children.Add(child);
			if (child.Parent != this)
				child.Parent = this;
		}
	}
}