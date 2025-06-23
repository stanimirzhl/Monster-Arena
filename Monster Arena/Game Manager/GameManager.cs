using Monster_Arena.Characters;
using Monster_Arena.Characters.Factories;
using Monster_Arena.Characters.Monster_Factory;
using Monster_Arena.Characters.Monsters;
using Monster_Arena.Events;
using Monster_Arena.Inventory;
using Monster_Arena.UI;
using System.IO.Compression;
using System.Reflection.Emit;
using System.Threading;
using static Monster_Arena.Characters.Monster_Factory.MonsterFactory;
using static Monster_Arena.UI.ConsoleUI;

namespace Monster_Arena
{
	public class GameManager
	{
		private static GameManager _instance;
		public static GameManager Instance => _instance ??= new GameManager();

		public Player Player { get; private set; }
		public Monster CurrentMonster { get; private set; }
		private int TurnCount;
		private int MonstersKilled = 0;
		public bool IsSkipped { get; set; } = false;
		private Random random = new Random();

		private List<string> GameHistory = new List<string>();

		private void LogGameEvent(string message)
		{
			if (GameHistory.Count == 3)
			{
				GameHistory.RemoveAt(0);
			}

			GameHistory.Add($"{DateTime.Now:HH:mm:ss} - {message}");
		}

		private void SaveGameHistoryToFile()
		{
			string filePath = "../../../Game History/GameHistory.txt";
			try
			{
				File.WriteAllLines(filePath, GameHistory);
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error saving game history: {ex.Message}");
				Console.ResetColor();
			}
		}


		public void Run()
		{
			ShowLoadingScreen();
			bool exitGame = false;

			while (!exitGame)
			{
				int mainChoice = ShowMainMenu();

				switch (mainChoice)
				{
					case 0:
						StartNewGame();
						break;
					case 1:
						LoadGameFromGZip("../../../Saved Games/MonsterArenaSave.gz");
						GameLoop();
						Thread.Sleep(1500);
						break;
					case 2:
						Console.Clear();
						Console.WriteLine("👋 Exiting... Bye!", ConsoleColor.Cyan);
						Thread.Sleep(1000);
						exitGame = true;
						break;
				}
			}
		}

		private static void ShowLoadingScreen()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;

			string[] title = new string[]
			{
				@"   __  __                 _             _               ",
				@"  |  \/  | ___  _ __ ___ | | ___   __ _(_)_ __ ___      ",
				@"  | |\/| |/ _ \| '_ ` _ \| |/ _ \ / _` | | '_ ` _ \     ",
				@"  | |  | | (_) | | | | | | | (_) | (_| | | | | | | |    ",
				@"  |_|  |_|\___/|_| |_| |_|_|\___/ \__, |_|_| |_| |_|    ",
				@"                                 |___/                ",
				@"		 WELCOME TO THE MONSTER ARENA!                 "
			};

			foreach (string line in title)
			{
				Console.WriteLine(line);
			}

			Console.ResetColor();
			Console.Write("\nLoading");

			for (int i = 0; i < 3; i++)
			{
				Thread.Sleep(500);
				Console.Write(".");
			}

			Thread.Sleep(800);
		}

		private static int ShowMainMenu()
		{
			string[] options = { "New Game", "Load Game", "Exit" };
			int index = 0;

			ConsoleKey key;

			do
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("======== Main Menu ========");
				Console.ResetColor();

				for (int i = 0; i < options.Length; i++)
				{
					if (i == index)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"> {options[i]}");
						Console.ResetColor();
					}
					else
					{
						Console.WriteLine($"  {options[i]}");
					}
				}

				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Press 'Enter' to continue..");
				Console.WriteLine("Move around with arrow keys");
				Console.ResetColor();

				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.UpArrow && index > 0)
					index--;
				else if (key == ConsoleKey.DownArrow && index < options.Length - 1)
					index++;

			} while (key != ConsoleKey.Enter);

			return index;
		}

		private void StartNewGame()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("🗡️ Starting New Game...");
			Console.ResetColor();
			Thread.Sleep(1000);
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("╔══════════════════════════════════════╗");
			Console.WriteLine("║      Welcome to Monster Arena!       ║");
			Console.WriteLine("╚══════════════════════════════════════╝");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\nIn this game, you will battle various monsters to gain experience and level up.");
			Console.WriteLine("Use items from your inventory to survive and become stronger.");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("\nBefore we begin, please enter your warrior name.");
			Console.WriteLine("Press any key to continue...");
			Console.ResetColor();
			Console.ReadKey(true);
			Console.Clear();

			Console.Write("🗡️ Enter your warrior name: ");
			string playerName = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(playerName))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid name. Returning to main menu.");
				Console.ResetColor();
				Thread.Sleep(1500);
				return;
			}

			Player = new Player(playerName);
			Player.Inventory.PickUpItem(new ResurrectingFeather());

			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"\nWelcome, {Player.Name}! Prepare for battle!");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("You have received a special item: 'Resurrecting Feather' 🪶");
			Console.WriteLine("This item will automatically revive you once upon death.");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("\nPress any key to begin your first battle...");
			Console.ResetColor();
			Console.ReadKey(true);

			TurnCount = 0;
			CurrentMonster = CreateRandomMonster(Player.Level + 1);

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"\n⚔️ Your first opponent is: {CurrentMonster.Name}!");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("Press any key to begin the battle...");
			Console.ResetColor();
			Console.ReadKey(true);

			GameLoop();
		}

		private void GameLoop()
		{
			while (Player.IsAlive)
			{
				TurnCount++;

				if (MonstersKilled % 3 == 0 && MonstersKilled != 0 && !IsSkipped)
				{
					GiveItemChoice();
				}

				bool turnEnded = false;

				do
				{
					int action = ShowBattleMenu();

					switch (action)
					{
						case 0:
							PlayerTurnAttack();
							LogGameEvent($"Player attacked {CurrentMonster.Name}.");
							turnEnded = true;
							break;
						case 1:
							UseItemTurn();
							break;
						case 2:
							ViewStatsTurn();
							break;
						case 3:
							ShowGameHistory();
							break;
						case 4:
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine("Returning to main menu...");
							Console.ResetColor();
							Thread.Sleep(1000);
							SaveGameToGZip("../../../Saved Games/MonsterArenaSave.gz");
							return;
					}
					SaveGameHistoryToFile();

				} while (!turnEnded);

				if (!CurrentMonster.IsAlive)
				{
					IsSkipped = false;
					MonstersKilled++;
					LogGameEvent($"Monster {CurrentMonster.Name} defeated!");
					Player.GainXP(CurrentMonster.XPReward);
					Thread.Sleep(1500);

					CurrentMonster = CreateRandomMonster(Player.Level + 1);

					float baseHeal = 15f;
					float levelScaling = 1.05f;
					int healAmount = Math.Min((int)(baseHeal * Math.Pow(levelScaling, Player.Level - 1)), 350);

					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine($"\n✨ Congratulations on your victory! You are healed for {healAmount} HP.");
					Console.ResetColor();

					Player.Heal(healAmount);

					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"\n⚔️ Your next opponent is: {CurrentMonster.Name}!");
					Console.ResetColor();
					LogGameEvent($"New monster {CurrentMonster.Name} appeared.");

					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("Press any key to continue...");
					Console.ResetColor();
					Console.ReadKey(true);
					continue;
				}

				if (Player.IsAlive)
				{
					MonsterTurn();
				}

				if (!Player.IsAlive)
				{
					HandlePlayerDeath();
					return;
				}
			}
		}

		private void PlayerTurnAttack()
		{
			Console.Clear();
			Player.Attack(CurrentMonster);
			Thread.Sleep(1000);
		}

		private void UseItemTurn()
		{
			if (Player.Inventory.NumberOfItems() == 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Your inventory is empty.");
				Console.ResetColor();
				Thread.Sleep(1500);
				return;
			}

			int itemIndex = ShowItemSelectionMenu();
			if (itemIndex == -1)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Cancelled item use.");
				Console.ResetColor();
				Thread.Sleep(1000);
				return;
			}

			Item selectedItem = Player.Inventory[itemIndex];

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Using item: {selectedItem.Name}");
			Console.ResetColor();
			LogGameEvent($"Player used item: {selectedItem.Name} - {selectedItem.Description}");
			selectedItem.Use(Player);
			Thread.Sleep(1500);
		}

		private void ViewStatsTurn()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("PLAYER STATS");
			Console.ResetColor();
			Console.WriteLine($"Name: {Player.Name}");
			Console.WriteLine($"Level: {Player.Level}");
			Console.WriteLine($"XP: {Player.XP}/{Player.NextLevelXP}");
			Console.WriteLine($"Health: {Player.Health}/{Player.MaxHealth}");
			Console.WriteLine($"XP till next level: {Player.NextLevelXP - Player.XP}");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("Press ESC to return to battle menu...");
			Console.ResetColor();
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
		}

		private void ShowGameHistory()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("🕒 Last 3 Game Events:");
			Console.ResetColor();

			if (GameHistory.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("No events have been logged yet.");
				Console.ResetColor();
			}
			else
			{
				foreach (var log in GameHistory)
				{
					Console.WriteLine(log);
				}
			}

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("\nPress any key to return to the battle menu...");
			Console.ResetColor();
			Console.ReadKey(true);
		}

		private void MonsterTurn()
		{
			Console.Clear();
			CurrentMonster.Attack(Player);
			Thread.Sleep(1000);
		}

		private void HandlePlayerDeath()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("You have been defeated!", ConsoleColor.Red);
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("Game Over!");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("Press any key to return to main menu...");
			Console.ResetColor();
			Console.ReadKey(true);
		}

		private void GiveItemChoice()
		{
			LogGameEvent($"Player reached {MonstersKilled} kills, giving item choice.");
			Item newItem = ItemFactory.CreateRandomItem();
			bool itemHandled = false;
			IsSkipped = true;

			while (!itemHandled)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"🎁 You found a new item: {newItem.Name} - {newItem.Description}");
				Console.ResetColor();

				if (Player.Inventory.IsFull)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("⚠️ Your inventory is full! You cannot pick up this item.");
					Console.ResetColor();
				}

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("Press 'E' to equip the item, 'I' to open inventory, or 'S' to skip.");
				Console.ResetColor();

				ConsoleKey choice;
				do
				{
					choice = Console.ReadKey(true).Key;
				} while (
					choice != ConsoleKey.I &&
					choice != ConsoleKey.S &&
					(choice != ConsoleKey.E || Player.Inventory.IsFull)
				);

				if (choice == ConsoleKey.S)
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine($"You chose not to pick up: {newItem.Name}.");
					Console.ResetColor();
					Thread.Sleep(1500);
					itemHandled = true;
				}
				else if (choice == ConsoleKey.I)
				{
					int index = ShowItemSelectionMenu();
					if (index == -1)
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.WriteLine("You exited inventory. Returning to item screen.");
						Console.ResetColor();
						Thread.Sleep(1000);
					}
					else
					{
						Item selected = Player.Inventory[index];
						selected.Use(Player);
						Player.Inventory.DropItemAt(index);

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{selected.Name} used and removed from inventory.");
						Console.ResetColor();
						Thread.Sleep(1500);
					}
				}
				else if (choice == ConsoleKey.E && !Player.Inventory.IsFull)
				{
					Player.Inventory.PickUpItem(newItem);
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"✅ You received: {newItem.Name}!");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(newItem.Description);
					Console.ResetColor();
					Thread.Sleep(2500);
					itemHandled = true;
				}
			}
		}

		private int ShowBattleMenu()
		{
			string[] options = { "Attack", "Use Item", "View Stats", "View History", "Quit to Menu" };
			int index = 0;
			ConsoleKey key;

			do
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Choose your action:");
				Console.ResetColor();

				for (int i = 0; i < options.Length; i++)
				{
					if (i == index)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"> {options[i]}");
						Console.ResetColor();
					}
					else
					{
						Console.WriteLine($"  {options[i]}");
					}
				}

				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Use arrow keys ↑ ↓ to move, Enter to select.");
				Console.ResetColor();
				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.UpArrow && index > 0) index--;
				else if (key == ConsoleKey.DownArrow && index < options.Length - 1) index++;

			} while (key != ConsoleKey.Enter);

			return index;
		}

		private int ShowItemSelectionMenu()
		{
			int index = 0;
			ConsoleKey key;

			do
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("🎒 Your Inventory:");
				Console.ResetColor();

				for (int i = 0; i < Player.Inventory.NumberOfItems(); i++)
				{
					if (i == index)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"> {Player.Inventory[i].Name} - {Player.Inventory[i].Description}");
						Console.ResetColor();
					}
					else
					{
						Console.WriteLine($"  {Player.Inventory[i].Name} - {Player.Inventory[i].Description}");
					}
				}

				if (Player.Inventory.NumberOfItems() == 0)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Your inventory is empty.");
					Console.ResetColor();

					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("Press Esc to return to the battle menu...");
					Console.ResetColor();

					key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
						return -1;

					continue;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("Use ↑ ↓ to move, Enter to select, D to drop, Esc to cancel.");
					Console.ResetColor();
				}

				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.UpArrow && index > 0) index--;
				else if (key == ConsoleKey.DownArrow && index < Player.Inventory.NumberOfItems() - 1) index++;
				else if (key == ConsoleKey.D && Player.Inventory.NumberOfItems() > 0)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"Are you sure you want to drop {Player.Inventory[index].Name}? (Y/N)");
					Console.ResetColor();
					if (Console.ReadKey(true).Key == ConsoleKey.Y)
					{
						string droppedItemName = Player.Inventory[index].Name;
						Player.Inventory.DropItemAt(index);
						Console.WriteLine("Item dropped.");
						LogGameEvent($"Player dropped item: {droppedItemName}");
						Thread.Sleep(1000);
						if (index >= Player.Inventory.NumberOfItems()) index = Math.Max(0, Player.Inventory.NumberOfItems() - 1);
					}
				}
				else if (key == ConsoleKey.Escape)
					return -1;

			} while (key != ConsoleKey.Enter || Player.Inventory.NumberOfItems() == 0);

			return index;
		}

		private void SaveGameToGZip(string filePath)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Create))
			using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
			using (StreamWriter writer = new StreamWriter(gzip))
			{
				writer.WriteLine($"PlayerName={Player.Name}");
				writer.WriteLine($"Level={Player.Level}");
				writer.WriteLine($"Health={Player.Health}");
				writer.WriteLine($"MaxHealth={Player.MaxHealth}");
				writer.WriteLine($"Inventory={string.Join(",", Player.Inventory.GetItems().Select(i => i.Name))}");
				writer.WriteLine($"XP={Player.XP}");
				writer.WriteLine($"NextLevelXP={Player.NextLevelXP}");
				writer.WriteLine($"AttackPower={Player.AttackPower}");
				writer.WriteLine($"MonstersKilled={MonstersKilled}");
				writer.WriteLine($"IsAlive={Player.IsAlive}");
				writer.WriteLine($"IsShoked={Player.IsShocked}");
				writer.WriteLine($"BurningRoundsRemaining={Player.BurningRoundsRemaining}");
				writer.WriteLine($"BonusXPRoundsRemaining={Player.BonusXPRoundsRemaining}");
				writer.WriteLine($"HasDamageReduction={Player.HasDamageReduction}");
				writer.WriteLine($"HasRevive={Player.HasRevive}");
				writer.WriteLine($"MonstersKilled={MonstersKilled}");

				if (CurrentMonster != null)
				{
					writer.WriteLine($"LastMonsterName={CurrentMonster.Name}");
					writer.WriteLine($"LastMonsterHealth={CurrentMonster.Health}");
					writer.WriteLine($"LastMonsterMaxHealth={CurrentMonster.MaxHealth}");
					writer.WriteLine($"LastMonsterLevel={CurrentMonster.Level}");
					writer.WriteLine($"LastMonsterXPReward={CurrentMonster.XPReward}");
					writer.WriteLine($"LastMonsterAttack={CurrentMonster.AttackPower}");
				}
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Game saved to compressed file successfully!");
			Console.ResetColor();
		}
		private void LoadGameFromGZip(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Save file not found.");
				Console.ResetColor();
				Thread.Sleep(1500);
				return;
			}

			using (FileStream fs = new FileStream(filePath, FileMode.Open))
			using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
			using (StreamReader reader = new StreamReader(gzip))
			{
				string line;
				Dictionary<string, string> data = new Dictionary<string, string>();

				while ((line = reader.ReadLine()) != null)
				{
					string[] parts = line.Split('=', 2);
					if (parts.Length == 2)
					{
						data[parts[0]] = parts[1];
					}
				}

				Player = new Player(data["PlayerName"]);
				Player.LoadState(data);

				MonstersKilled = int.Parse(data["MonstersKilled"]);

				string[] inventoryItems = data["Inventory"].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Player.Inventory.DropAllItems();
				foreach (string itemName in inventoryItems)
				{
					Item item = ItemFactory.CreateItem(itemName.Trim());
					if (item != null)
						Player.Inventory.PickUpItem(item);
				}

				if (data.ContainsKey("LastMonsterName"))
				{
					Monster monster = MonsterFactory.CreateMonster(
						data["LastMonsterName"],
						int.Parse(data["LastMonsterLevel"])
					);

					monster.LoadState(
						int.Parse(data["LastMonsterHealth"]),
						int.Parse(data["LastMonsterMaxHealth"]),
						int.Parse(data["LastMonsterXPReward"]),
						int.Parse(data["LastMonsterAttack"])
					);

					CurrentMonster = monster;
				}
				else
				{
					CurrentMonster = null;
				}
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Game loaded successfully from compressed file!");
			Console.ResetColor();
			Thread.Sleep(1500);
		}
	}
}
