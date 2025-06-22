namespace Monster_Arena.Characters
{
	public abstract class Character
	{
		protected Random random = new Random();

		private string name;

		public string Name
		{
			get { return name; }
			protected set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("Name of character cannot be null or empty!");
				}
				name = value;
			}
		}

		private int health;

		public int Health
		{
			get { return health; }
			protected set
			{
				health = value;
			}
		}

		private int maxHealth;

		public int MaxHealth
		{
			get { return maxHealth; }
			protected set
			{
				if (value < 1)
				{
					throw new ArgumentException("Max health cannot be lower than 1!");
				}
				maxHealth = value;
			}
		}


		private int attackPower;

		public int AttackPower
		{
			get { return attackPower; }
			protected set
			{
				if (value < 0)
				{
					throw new ArgumentException("Attack power cannot be lower than 0!");
				}
				attackPower = value;
			}
		}

		private int level;

		public int Level
		{
			get { return level; }
			protected set
			{
				if (value < 1)
				{
					throw new ArgumentException("Level cannot be lower than 1!");
				}
				level = value;
			}
		}

		public bool IsAlive => Health > 0;

		protected Character(string name,int maxHealth, int attackPower, int level)
		{
			this.Name = name;
			this.MaxHealth = maxHealth;
			this.Health = MaxHealth;
			this.AttackPower = attackPower;
			this.Level = level;
		}

		public abstract void Attack(Character target);

		public virtual void Defend(int damage)
		{
			Health -= damage;

			if (Health < 0)
			{
				Health = 0;
			}
		}

		public void Heal(int amount)
		{
			Health = Math.Min(Health + amount, MaxHealth);
		}

		protected int CalculateHealthCap(int baseHealth, int level, int maxHealth)
			=> (int)Math.Min(baseHealth + (level - 1) * 3.5, maxHealth);

		protected int CalculateAttackCap(int baseAttack, int level, int maxAttack)
			=> (int)Math.Min(baseAttack + (level - 1) * 1.5, maxAttack);
	}
}
