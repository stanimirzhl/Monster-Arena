using Monster_Arena.Events;
using Monster_Arena.Inventory;
using Monster_Arena.UI;

namespace Monster_Arena.Characters.Monsters
{
	public class Goblin : Monster
	{
		public Goblin(int level) : base("Goblin", 25, 5, level, (int)(10 + (2.5 * level)))
		{
			MaxHealth = CalculateHealthCap(25, level, 485);
			Health = MaxHealth;
			AttackPower = CalculateAttackCap(5, level, 150);
		}

		public override void Attack(Character target)
		{
			int chance = (int)Math.Min(5 + (Level - 1) * 0.2, 15);

			int damage = AttackPower;

			int critChance = (int)Math.Min(5 + (Level - 1) * 0.1, 10);
			if (random.Next(100) < critChance)
			{
				damage *= 2;
				GameEvents.RaiseCriticalHit(this, target);
			}

			if (random.Next(100) < chance)
			{
				damage = AttackPower + 5;

				StealItem(target as Player);
			}
			else
			{
				ConsoleUI.BasicAttack(this, target);
			}
			target.Defend(damage);
		}

		public override void Defend(int damage)
		{
			base.Defend(damage);

			ConsoleUI.TakeDamage(this, damage);
		}

		private void StealItem(Player player)
		{
			if(player.Inventory.NumberOfItems() == 0)
			{
				ConsoleUI.UnsuccessfulSteal(player);
			}
			else
			{
				Item stolenItem = player.Inventory.StealRandomItem();

				ConsoleUI.SneakAttack(this, player, stolenItem);
			}
		}
	}
}
