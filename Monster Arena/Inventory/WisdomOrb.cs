using Monster_Arena.Characters;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Monster_Arena.Inventory
{
	public class WisdomOrb : Item
	{
		public WisdomOrb() : base("Wisdom Orb", "When used, grants 50 % bonus XP for the next 3 battles.")
		{
		}

		public override void Use(Player player)
		{
			player.BonusXPRoundsRemaining = 3;

			player.Inventory.DropItem(this);
			ConsoleUI.UseItem(this, player);
		}
	}
}
