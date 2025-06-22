using Monster_Arena.Events;
using Monster_Arena.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Monster_Arena.Characters.Monsters
{
	public class Dragon : Monster
	{
		public Dragon(int level) : base("Dragon", 55, 20, level, (int)(30 + (2.5 * level)))
		{
			MaxHealth = CalculateHealthCap(55, level, 1200);
			Health = MaxHealth;
			AttackPower = CalculateAttackCap(20, level, 350);
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

				Fire(target as Player);
			}
			else
			{
				ConsoleUI.BasicAttack(this, target);
			}
			target.Defend(damage);
		}

		public override void Defend(int damage)
		{
			int chance = (int)Math.Min(5 + (Level - 1) * 0.2, 15);
			if (random.Next(100) < chance)
			{
				damage = (int)(damage * 0.5);
				ConsoleUI.Scales(this, damage);

				base.Defend(damage);
				return;
			}

			base.Defend(damage);
			ConsoleUI.TakeDamage(this, damage);
		}

		private void Fire(Player player)
		{
			player.BurningRoundsRemaining = 2;

			ConsoleUI.FireBreath(this, player);
		}
	}
}
