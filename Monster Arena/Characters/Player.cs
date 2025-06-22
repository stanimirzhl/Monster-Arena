using Monster_Arena.Characters.Monsters;
using Monster_Arena.Events;
using Monster_Arena.Inventory;
using Monster_Arena.UI;

namespace Monster_Arena.Characters
{
	public class Player : Character
	{
		public int XP { get; private set; } = 0;
		public int NextLevelXP { get; private set; } = 100;
		public Bag<Item> Inventory { get; set; } = new Bag<Item>();

		public bool HasDamageReduction { get; set; }
		public bool HasRevive { get; set; }
		public int BonusXPRoundsRemaining { get; set; }
		public bool IsShocked { get; set; }
		public int BurningRoundsRemaining { get; set; } = 0;
		public bool IsBurning => BurningRoundsRemaining > 0;

		public Player(string name) : base(name, 100, 20, 1)
		{
			MaxHealth = base.CalculateHealthCap(100, 1, 1500);
			this.Health = MaxHealth;
			AttackPower = base.CalculateAttackCap(20, 1, 300);
		}

		public override void Attack(Character target)
		{
			if (IsShocked)
			{
				ConsoleUI.Stunned(this);
				IsShocked = false;
				return;
			}

			if (IsBurning)
			{
				int burnDamage = 10;
				Defend(burnDamage);
				BurningRoundsRemaining--;
				ConsoleUI.ShowFireBreathDamage(this, burnDamage, BurningRoundsRemaining);
			}

			int damage = AttackPower;

			int chance = (int)Math.Min(5 + (Level - 1) * 0.1, 20);
			if (random.Next(100) < chance)
			{
				damage *= 2;

				GameEvents.RaiseCriticalHit(this, target);
			}
			else
			{
				ConsoleUI.BasicAttack(this, target);
			}

			target.Defend(damage);

			if (!target.IsAlive)
			{
				ConsoleUI.DefeatMonster(target as Monster, this);
			}
		}

		public override void Defend(int damage)
		{
			if (HasDamageReduction)
			{
				damage = (int)(damage * 0.2);
			}
			base.Defend(damage);

			ConsoleUI.TakeDamage(this, damage);

			if (!IsAlive && HasRevive)
			{
				Health = (int)(100 * Math.Pow(1.1, Level));
				HasRevive = false;

				Task.Delay(1500).Wait();
				ConsoleUI.Revive(this);
			}
		}

		public void GainXP(int amount)
		{
			if (BonusXPRoundsRemaining > 0)
			{
				amount = (int)(amount * 1.5);
				BonusXPRoundsRemaining--;
			}

			XP += amount;

			while (XP >= NextLevelXP)
			{
				XP -= NextLevelXP;
				Level++;

				int nextLvlHealth = (int)(100 * Math.Pow(1.15, Level - 1));
				int nextLvlPower = (int)(20 * Math.Pow(1.15, Level - 1));

				NextLevelXP = CalculateNextLevelXP();

				GameEvents.RaiseLevelUp(this);

				MaxHealth = Math.Min(nextLvlHealth, 1500);
				AttackPower = Math.Min(nextLvlPower, 300);

				this.Health = MaxHealth;
			}
		}
		private int CalculateNextLevelXP()
		{
			return (int)(100 * (2.5 * Level));
		}

		public void LoadState(Dictionary<string, string> data)
		{
			this.Level = int.Parse(data["Level"]);
			this.Health = int.Parse(data["Health"]);
			this.MaxHealth = int.Parse(data["MaxHealth"]);
			this.XP = int.Parse(data["XP"]);
			this.NextLevelXP = int.Parse(data["NextLevelXP"]);
			this.AttackPower = int.Parse(data["AttackPower"]);
			this.IsShocked = bool.Parse(data["IsShoked"]);
			this.BurningRoundsRemaining = int.Parse(data["BurningRoundsRemaining"]);
			this.BonusXPRoundsRemaining = int.Parse(data["BonusXPRoundsRemaining"]);
			this.HasDamageReduction = bool.Parse(data["HasDamageReduction"]);
			this.HasRevive = bool.Parse(data["HasRevive"]);
		}
	}
}
