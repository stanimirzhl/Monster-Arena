using Monster_Arena.Events;
using Monster_Arena.Inventory;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Characters.Monsters
{
	public class Orc : Monster
	{
		public Orc(int level) : base("Orc", 45, 15, level, (int)(15 + (2.5 * level)))
		{
			MaxHealth = CalculateHealthCap(45, level, 890);
			Health = MaxHealth;
			AttackPower = CalculateAttackCap(15, level, 250);
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
				damage = AttackPower + 20;

				Shock(target as Player);
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

		private void Shock(Player player)
		{
			player.IsShocked = true;

			ConsoleUI.ShockAttack(player);
		}
	}
}
