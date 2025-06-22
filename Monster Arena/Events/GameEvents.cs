using Monster_Arena.Characters;

namespace Monster_Arena.Events
{
	public class GameEvents
	{
		public delegate void LevelUpHandler(Character character);
		public static event LevelUpHandler OnLevelUp;

		public delegate void CriticalHitHandler(Character attacker, Character target);
		public static event CriticalHitHandler OnCriticalHit;

		public static void RaiseLevelUp(Character character)
		{
			OnLevelUp?.Invoke(character);
		}

		public static void RaiseCriticalHit(Character attacker, Character target)
		{
			OnCriticalHit?.Invoke(attacker, target);
		}
	}
}
