using System;
using System.Collections;
using System.Collections.Generic;

namespace SalePlanner.Domain
{
	/// <summary>
	/// Class represents single territorial region in hierarchy.
	/// It will be used as one of dimension for sale planning.
	/// </summary>
	public class Region : IEnumerable<Region>
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

		/// <summary>
		/// Finds first region which meets the specified criteria including.
		/// </summary>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public Region Find(Predicate<Region> criteria)
		{
			foreach(var item in this)
			{
				if (criteria(item))
					return item;
			}
			return null;
		}

		/// <summary>
		/// Finds all region which meets the specified criteria.
		/// </summary>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public List<Region> FindAll(Predicate<Region> criteria)
		{
			var result = new List<Region>();
			foreach(var item in this)
			{
				if (criteria(item))
					result.Add(item);
			}
			return result.Count > 0 ? result : null;
		}

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Region>) this).GetEnumerator();
		}

		#endregion

		#region IEnumerable<Region> Members

		public IEnumerator<Region> GetEnumerator()
		{
			yield return this;
			foreach(var item in Children)
			{
				IEnumerator<Region> enumerator = item.GetEnumerator();
				while (enumerator.MoveNext())
					yield return enumerator.Current;
			}
		}

		#endregion
	}
}