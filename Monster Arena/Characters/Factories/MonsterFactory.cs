using Monster_Arena.Characters.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Arena.Characters.Monster_Factory
{
	public static class MonsterFactory
	{
		public static Monster CreateMonster(string name, int level)
		{
			return name.ToLower() switch
			{
				"dragon" => new Dragon(level),
				"goblin" => new Goblin(level),
				"orc" => new Orc(level),
				_ => throw new ArgumentException($"Monster '{name}' not recognized"),
			};
		}

		public static Monster CreateRandomMonster(int level)
		{
			string[] monsters = { "Dragon", "Goblin", "Orc" };
			var rnd = new Random();
			return CreateMonster(monsters[rnd.Next(monsters.Length)], level);
		}
	}
}
