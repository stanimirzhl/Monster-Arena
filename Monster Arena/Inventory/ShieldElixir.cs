using Monster_Arena.Characters;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public class ShieldElixir : Item
	{
		public ShieldElixir() : base("Shield Elixir", "Blocks enemy attack by 80%")
		{
		}

		public override void Use(Player player)
		{
			player.HasDamageReduction = true;
			ConsoleUI.UseItem(this, player);
		}
	}
}
