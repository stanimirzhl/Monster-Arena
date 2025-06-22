using Monster_Arena.Characters;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Inventory
{
	public class HealingPotion : Item
	{
		public HealingPotion() : base("Healing Potion", "When drank, restores 20% of players max health.")
		{
		}

		public string PlayerName { get; set; }
		public int HealAmount { get; set; }

		public override void Use(Player player)
		{
			int healAmount = (int)(player.MaxHealth * 0.2);
			int previousHealth = player.Health;
			player.Heal(healAmount);
			int actualHealed = player.Health - previousHealth;

			this.PlayerName = player.Name;
			this.HealAmount = actualHealed;

			player.Inventory.DropItem(this);
			ConsoleUI.UseItem(this, player);
		}
	}
}
