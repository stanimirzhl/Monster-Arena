using Monster_Arena.Characters;
using Monster_Arena.Inventory;

namespace Monster_Arena.UI
{
	public static class ConsoleUI
	{
		public static void Separator() => Console.WriteLine(new string('=', 40));

		public static void LevelUp(Character character)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\n🎉 {character.Name} just leveled up to Level {character.Level}!");
			Console.ResetColor();
		}

		public static void CriticalHit(Character attacker, Character target, int damage)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"\n🔥 Critical hit! {attacker.Name} dealt {damage} damage to {target.Name}!");
			Console.ResetColor();
		}

		public static void BasicAttack(Character attacker, Character target, int damage)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($"\n🗡️{attacker.Name} attacks {target.Name} for {damage} damage.");
			Console.ResetColor();
		}

		public static void SneakAttack(Character attacker, Character target, int damage)
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine($"\n👻 {attacker.Name} performs a sneak attack on {target.Name} for {damage} damage!");
			Console.ResetColor();
		}

		public static void UseItem(Item item, Player player)
		{
			switch (item)
			{
				case ShieldElixir:
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"\n🛡️ {player.Name} used a Shield Elixir! Damage reduction is now active.");
					Console.ResetColor();
					break;
				default:
					break;
			}
		}

		public static void Clear() => Console.Clear();
	}
}
