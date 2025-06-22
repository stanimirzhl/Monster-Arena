using Monster_Arena;
using Monster_Arena.Characters;
using Monster_Arena.Characters.Monsters;
using Monster_Arena.Events;
using Monster_Arena.Inventory;
using Monster_Arena.UI;
using static Monster_Arena.UI.ConsoleUI;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

GameEvents.OnLevelUp += character =>
{
	LevelUp(character);
};

GameEvents.OnCriticalHit += (attacker, target) =>
{
	CriticalHit(attacker, target);
};

try
{
	GameManager.Instance.Run();
}
catch (Exception ex)
{
	Console.WriteLine("Error: " + ex.Message);
}


