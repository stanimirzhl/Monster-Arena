using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public class Bag<T> where T : IName
	{
		private List<T> items;

		private const int MaxCapacity = 5;

		public bool IsFull => items.Count >= MaxCapacity;

		public Bag()
		{
			items = new List<T>();
		}

		public bool PickUpItem(T item)
		{
			if (!IsFull)
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

		public bool DropItemAt(int index)
		{
			T item = items[index];

			return this.DropItem(item);
		}

		public T StealRandomItem()
		{
			int index = new Random().Next(items.Count);
			T item = items[index];
			items.RemoveAt(index);
			return item;
		}

		public T GetItem(string name)
		{
			return items.FirstOrDefault(item => item.Name == name);
		}

		public void DropAllItems() => items.Clear();

		public int NumberOfItems() => items.Count;

		public IEnumerable<T> GetItems() => items;

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= items.Count)
					throw new IndexOutOfRangeException("Index out of range.");
				return items[index];
			}
			set
			{
				if (index < 0 || index >= items.Count)
					throw new IndexOutOfRangeException("Index out of range.");
				items[index] = value;
			}
		}
	}
}
