using Monster_Arena.Events;
using Monster_Arena.UI;

namespace Monster_Arena.Characters
{
	public class Player : Character
	{
		public int XP { get; private set; } = 0;
		public int NextLevelXP { get; private set; } = 100;

        public bool HasDamageReduction { get; set; }

        public Player(string name) : base(name, 100, 20, 1)
		{
		}

		public override void Attack(Character target)
		{
			int damage = AttackPower;

			int chance = (int)Math.Min(5 + (Level - 1) * 0.1, 20);

			if (random.Next(100) < chance)
			{
				damage *= 2;

				GameEvents.RaiseCriticalHit(this, target, damage);
			}
			else
			{
				ConsoleUI.BasicAttack(this, target, damage);
			}

			target.Defend(damage);
		}

		public void GainXP(int amount)
		{
			XP += amount;

			while (XP >= NextLevelXP)
			{
				XP -= NextLevelXP;
				Level++;

				Health += (int)(100 * Math.Pow(1.15, Level - 1));
				AttackPower += (int)(20 * Math.Pow(1.15, Level - 1));

				NextLevelXP = CalculateNextLevelXP();

				GameEvents.RaiseLevelUp(this);
			}
		}

		private int CalculateNextLevelXP()
		{
			return (int)(100 * Math.Pow(1.15, Level));
		}
	}
}
