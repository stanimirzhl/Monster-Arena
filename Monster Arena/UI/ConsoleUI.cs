using Monster_Arena.Characters;
using Monster_Arena.Characters.Monsters;
using Monster_Arena.Inventory;
using System.Xml.Linq;

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

		public static void CriticalHit(Character attacker, Character target)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"\n🔥 Critical hit! {attacker.Name} dealt double damage to {target.Name}!");
			Console.ResetColor();
		}

		public static void BasicAttack(Character attacker, Character target)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine($"\n🗡️{attacker.Name} attacks {target.Name}.");
			Console.ResetColor();
		}

		public static void TakeDamage(Character character, int damage)
		{
			Task.Delay(500).Wait();
			Console.WriteLine();

			string[] spinner = { "|", "/", "-", "\\" };
			int totalFrames = 25;
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write($"💥 {character.Name} is taking damage {spinner[0]}");
			for (int i = 0; i < totalFrames; i++)
			{
				Console.Write($"\b{spinner[i % spinner.Length]}");
				Task.Delay(80).Wait();
			}
			Console.Write("\r" + new string(' ', Console.WindowWidth));
			Console.Write("\r");
			Console.ResetColor();

			int totalBars = 20;
			double healthRatio = Math.Max(0, (double)character.Health / character.MaxHealth);
			int filledBars = (int)(totalBars * healthRatio);
			int emptyBars = totalBars - filledBars;

			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write($"{character.Name}'s Health: [");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(new string('█', filledBars));
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(new string('░', emptyBars));
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("]");
			Console.ResetColor();

			Task.Delay(500).Wait();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($" => {character.Name} took {damage} damage. Remaining HP: {character.Health}/{character.MaxHealth}\n");
			Console.ResetColor();

			Task.Delay(800).Wait();
		}

		public static void DefeatMonster(Monster monster, Player player)
		{
			string[] sparkle = { "*", "+", "x", "✦", "✧", "✪" };
			int sparkleFrames = 20;

			for (int i = 0; i < sparkleFrames; i++)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"\r✨ {sparkle[i % sparkle.Length]} You defeated {monster.Name}! {sparkle[i % sparkle.Length]} ✨");
				Thread.Sleep(80);
			}
			Console.WriteLine();
			Console.ResetColor();

			int totalBars = 30;
			player.GainXP(monster.XPReward);
			int newXP = player.XP;

			double ratio = (double)newXP / player.NextLevelXP;
			int filled = (int)(totalBars * ratio);
			int empty = totalBars - filled;

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("\n📈 Gaining XP: [");
			for (int i = 0; i < filled; i++)
			{
				Console.Write("█");
				Thread.Sleep(40);
			}
			Console.ForegroundColor = ConsoleColor.DarkGray;
			for (int i = 0; i < empty; i++)
			{
				Console.Write("░");
				Thread.Sleep(10);
			}
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("]");

			Console.ResetColor();
			Console.WriteLine($"✨ You gained {monster.XPReward} XP! Total XP: {player.XP}/{player.NextLevelXP} ✨");
		}

		public static void Revive(Character character)
		{
			string name = character.Name;
			int maxHealth = character.MaxHealth;
			int totalBars = 20;

			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"\n✨✨✨ {name} is being revived... ✨✨✨\n");
			Console.ResetColor();

			for (int i = 0; i < 6; i++)
			{
				Console.ForegroundColor = (i % 2 == 0) ? ConsoleColor.Yellow : ConsoleColor.White;
				Console.WriteLine(i % 2 == 0
					? "     ✨     ✨     ✨     ✨     ✨"
					: "  ✨     ✨     ✨     ✨     ✨  ");
				Task.Delay(250).Wait();
				Console.Clear();
				Console.WriteLine($"\n✨✨✨ {name} is being revived... ✨✨✨\n");
			}

			for (int filledBars = 0; filledBars <= totalBars; filledBars++)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write($"{name}'s Health: [");
				Console.Write(new string('█', filledBars));
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write(new string('░', totalBars - filledBars));
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("]");

				int currentHealth = (int)((double)filledBars / totalBars * maxHealth);
				Console.WriteLine($"\n❤️ Revived HP: {currentHealth}/{maxHealth}");
				Task.Delay(500).Wait();
			}

			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"\n✨ {name} has been fully revived and is ready to fight again! ✨");
			Console.ResetColor();

			Task.Delay(1000).Wait();
		}

		public static void SneakAttack(Character attacker, Character target, Item stolenItem)
		{
			string goblinIcon = "🧟";
			string victimIcon = "🧍";
			int steps = 10;

			Console.Clear();
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine($"\n👻 {attacker.Name} is sneaking toward {target.Name}...");

			for (int i = 0; i <= steps; i++)
			{
				Console.SetCursorPosition(0, 2);
				Console.Write(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(i, 2);
				Console.Write(goblinIcon);
				Console.SetCursorPosition(steps + 5, 2);
				Console.Write(victimIcon);
				Thread.Sleep(100);
			}

			Thread.Sleep(300);
			Console.SetCursorPosition(0, 4);
			Console.WriteLine($"💨 {attacker.Name} snatched a shiny item!");

			Thread.Sleep(850);

			Console.SetCursorPosition(0, 6);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"🎒 Stolen Item: {stolenItem.Name}");
			Console.ResetColor();

			for (int i = steps; i >= 0; i--)
			{
				Console.SetCursorPosition(0, 2);
				Console.Write(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(i, 2);
				Console.Write(goblinIcon);
				Console.SetCursorPosition(steps + 5, 2);
				Console.Write(victimIcon);
				Thread.Sleep(100);
			}

			Thread.Sleep(500);

			for (int i = 0; i < 10; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write(new string(' ', Console.WindowWidth));
			}

			Console.SetCursorPosition(0, 0);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine($"👀 No trace of the thief remains...");
			Console.ResetColor();
			Thread.Sleep(1200);
		}

		public static void ShockAttack(Player player)
		{
			Console.Clear();
			string orc = "🟫(Orc)🟫";
			string[] windEmojis = { "💨", "🌪️", "🌀" };

			int maxDistance = 12;
			int waveCount = 5;

			int windowWidth = Console.WindowWidth;
			int orcPos = (windowWidth - orc.Length) / 2;

			for (int wave = 0; wave < waveCount; wave++)
			{
				for (int distance = 0; distance <= maxDistance; distance++)
				{
					Console.Clear();

					string wind = windEmojis[distance % windEmojis.Length];

					int leftWindPos = orcPos - distance - 1;
					int rightWindPos = orcPos + orc.Length + distance;

					if (leftWindPos >= 0)
					{
						Console.SetCursorPosition(leftWindPos, Console.CursorTop);
						Console.Write(wind);
					}

					Console.SetCursorPosition(orcPos, Console.CursorTop);
					Console.Write(orc);

					if (rightWindPos < windowWidth)
					{
						Console.SetCursorPosition(rightWindPos, Console.CursorTop);
						Console.Write(wind);
					}

					Thread.Sleep(80);
				}
			}

			Console.Clear();

			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"\n⚡ {player.Name} is stunned by the orc's stomp shockwave and can't move on their next turn!");
			Console.ResetColor();

			Thread.Sleep(1200);
		}

		public static void Stunned(Character player)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"\n⚡ {player.Name} is stunned, unable to attack!");
			Console.ResetColor();
		}

		public static void FireBreath(Dragon dragon, Player player)
		{
			Console.Clear();

			string[] dragonArt = new string[]
			{
		"                   _______________                   ",
		"       ,===:'.,            `-._                      ",
		"            `:.`---.__         `-._                  ",
		"              `:.     `--.         `.                ",
		"                \\.        `.         `.              ",
		"        (,,(,    \\.         `.   ____,-`.,           ",
		"     (,'     `/   \\.   ,--.___`.'                    ",
		" ,  ,'  ,--.  `,   \\.;'         `                    ",
		"  `{D, {    \\  :    \\;                                 ",
		"    V,,'    /  /    //                                 ",
		"    j;;    /  ,' ,-//.    ,---.      ,                ",
		"    \\;'   /  ,' /  _  \\  /  _  \\   ,'/_              ",
		"          \\   `'  / \\  `'  / \\  `.'  /\\_\\            ",
		"           `.___,'   `.__,'   `.__,'                   "
			};

			int leftMargin = 10;
			int dragonY = 5;
			int mouthLineIndex = 10;
			int mouthOffset = 10;

			Console.ForegroundColor = ConsoleColor.DarkRed;
			for (int i = 0; i < dragonArt.Length; i++)
			{
				Console.SetCursorPosition(leftMargin, dragonY + i);
				Console.Write(dragonArt[i]);
			}
			Console.ResetColor();

			int mouthX = leftMargin + mouthOffset;
			int mouthY = dragonY + mouthLineIndex;

			int maxFireWidth = 8;
			Random rnd = new Random();

			for (int step = 1; step <= maxFireWidth; step *= 2)
			{
				for (int line = 0; line < 4; line++)
				{
					Console.SetCursorPosition(mouthX - maxFireWidth - 2, mouthY + line);
					Console.Write(new string(' ', maxFireWidth + 8));
				}

				int totalLines = (int)Math.Log(step, 2) + 1;
				int emojisToDraw = step;

				for (int line = 0; line < totalLines; line++)
				{
					int lineEmojis = Math.Max(1, emojisToDraw / (int)Math.Pow(2, totalLines - 1 - line));
					int spacesBefore = maxFireWidth - lineEmojis + rnd.Next(0, 2);

					string fire = string.Concat(Enumerable.Repeat("🔥", lineEmojis));
					int fireX = mouthX - spacesBefore - lineEmojis;

					Console.SetCursorPosition(fireX, mouthY + line);
					Console.Write(fire);
				}

				Thread.Sleep(300);
			}

			Thread.Sleep(1000);

			for (int line = 0; line < 4; line++)
			{
				Console.SetCursorPosition(mouthX - maxFireWidth - 2, mouthY + line);
				Console.Write(new string(' ', maxFireWidth + 8));
				Thread.Sleep(500);
			}

			Console.Clear();

			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"\n💥 {player.Name} is scorched and will take damage for the next 2 turns!");
			Console.ResetColor();

			Thread.Sleep(800);
		}


		public static void ShowFireBreathDamage(Player player, int damage, int roundsLeft)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"\n🔥 {player.Name} takes {damage} fire damage from burning! ({roundsLeft} rounds left)");
			Console.ResetColor();
		}

		public static void Scales(Character dragon, int reducedDamage)
		{
			string[] frames =
			{
				"🐉 <💥>   🛡️",
				"🐉  <🧱>  🛡️",
				"🐉   <✨> 🛡️",
				"🐉    <🧊>🛡️",
			};

			for (int i = 0; i < frames.Length; i++)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.WriteLine($"\n{frames[i]}");
				Thread.Sleep(500);
			}

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"\n🛡️ {dragon.Name}'s flake scales resist the attack! Only took {reducedDamage} damage.");
			Console.ResetColor();
			Thread.Sleep(800);
		}

		public static void UnsuccessfulSteal(Character player)
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine($"\n🧌👜 The Goblin tried to steal, but {player.Name} had nothing to steal!");
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
				case ResurrectingFeather:
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"\n🌟 {player.Name} now holds a Resurrection Feather. It will activate automatically upon death!");
					Console.ResetColor();
					break;
				case WisdomOrb:
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine($"\n🧿 {player.Name} activated The Wisdom Orb! +50% XP for the next 3 battles.");
					Console.ResetColor();
					break;
				case HealingPotion healingPotion:
					Console.ForegroundColor = ConsoleColor.Green;
					string message = $"{healingPotion.PlayerName} is healing... ";
					string[] spinner = { "|", "/", "-", "\\" };
					int totalFrames = 40;
					Console.Write(message + spinner[0]);
					for (int i = 0; i < totalFrames; i++)
					{
						Console.Write($"\b{spinner[i % spinner.Length]}");
						Task.Delay(30).Wait();
					}

					Console.Write("\r" + new string(' ', message.Length));
					Console.Write("\r");

					int totalBars = 20;
					double healthRatio = (double)player.Health / player.MaxHealth;
					int filledBars = (int)(totalBars * healthRatio);
					int emptyBars = totalBars - filledBars;

					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write("Healing Effect: [");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(new string('█', filledBars));
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(new string('░', emptyBars));
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine("]");
					Console.ResetColor();

					Task.Delay(500).Wait();

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($" => Restored {healingPotion.HealAmount} HP! Current HP: {player.Health}/{player.MaxHealth}");
					Console.ResetColor();
					break;
				default:
					break;
			}
		}
	}
}
