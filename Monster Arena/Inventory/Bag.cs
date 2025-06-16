using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public class Bag<T> where T : class
	{
		private List<T> items;

		public Bag()
		{
			items = new List<T>();
		}

		public bool PickUpItem(T item)
		{
			if (items.Count > 10)
			{
				items.Add(item);
				return true;
			}
			return false;
		}

		public bool DropItem(T item)
		{
			if (item != null && items.Contains(item))
			{
				return items.Remove(item);
			}
			return false;
		}

		public void DropAllItems() => items.Clear();

		public IEnumerable<T> GetItems() => items;
	}
}
