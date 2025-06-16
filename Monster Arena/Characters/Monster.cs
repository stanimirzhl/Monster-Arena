namespace Monster_Arena.Characters
{
	public abstract class Monster : Character
	{
		protected static Random random = new Random();

		protected Monster(string name, int baseHealth, int baseAttackPower, int level)
			: base(name,
				   (int)(baseHealth * Math.Pow(1.2, level - 1)),
				   (int)(baseAttackPower * Math.Pow(1.2, level - 1)),
				   level)
		{
		}
	}
}
