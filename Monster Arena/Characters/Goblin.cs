using Monster_Arena.UI;

namespace Monster_Arena.Characters
{
	public class Goblin : Monster
	{
		public Goblin(int level) : base("Goblin", 55, 10, level)
		{
		}

		public override void Attack(Character target)
		{
			int chance = (int)Math.Min(5 + (Level - 1) * 0.2, 15);

			int damage = AttackPower;

			if (random.Next(100) < chance)
			{
				damage = AttackPower + 5;
				ConsoleUI.SneakAttack(this, target, damage);
			}
			else
			{
				ConsoleUI.BasicAttack(this, target, damage);
			}
			target.Defend(damage);
		}
	}
}
