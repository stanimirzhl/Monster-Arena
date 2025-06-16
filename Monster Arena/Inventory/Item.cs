using Monster_Arena.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public abstract class Item
	{
		private string name;

		public string Name
		{
			get { return name; }
			protected set 
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("Name of the usable item cannot be null!");
				}
				name = value;
			}
		}

		private string description;

		public string Description
		{
			get { return description; }
			protected set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("Description of the usable item cannot be null!");
				}
				description = value;
			}
		}

        protected Item(string name, string description)
        {
            this.Name = name;
			this.Description = description;
		}

        public abstract void Use(Player player);
	}
}
