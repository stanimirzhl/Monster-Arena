using Monster_Arena.Characters;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public class ResurrectingFeather : Item
	{
		public ResurrectingFeather() : base("Resurrecting Feather", "1 time use only, brings the player back from the death!")
		{
		}

		public override void Use(Player player)
		{
			player.HasRevive = true;
			player.Inventory.DropItem(this);

			ConsoleUI.UseItem(this, player);
		}
	}
}
