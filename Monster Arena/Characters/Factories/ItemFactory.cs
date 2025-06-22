using Monster_Arena.Characters.Monsters;
using Monster_Arena.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Characters.Factories
{
	public static class ItemFactory
	{
		public static Item CreateItem(string name)
		{
			return name.ToLower() switch
			{
				"resurrecting feather" => new ResurrectingFeather(),
				"shield elixir" => new ShieldElixir(),
				"healing potion" => new HealingPotion(),
				_ => throw new ArgumentException($"Item '{name}' not recognized"),
			};
		}

		public static Item CreateRandomItem()
		{
			string[] items = { "Resurrecting Feather", "Shield Elixir", "Healing Potion" };
			var rnd = new Random();
			return CreateItem(items[rnd.Next(items.Length)]);
		}
	}
}
