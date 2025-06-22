namespace Monster_Arena.Characters.Monsters
{
	public abstract class Monster : Character
	{
		public int XPReward { get; protected set; }

		protected Monster(string name, int baseHealth, int baseAttackPower, int level, int xPReward)
		: base(name, baseHealth, baseAttackPower, level)
		{
			XPReward = xPReward;
		}

		public void LoadState(int health, int maxHealth, int xpReward, int attackPower)
		{
			this.Health = health;
			this.MaxHealth = maxHealth;
			this.XPReward = xpReward;
			this.AttackPower = attackPower;
		}
	}
}
