using Monster_Arena.Events;
using Monster_Arena.UI;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

GameEvents.OnLevelUp += character =>
{
	ConsoleUI.LevelUp(character);
};

GameEvents.OnCriticalHit += (attacker, target, damage) =>
{
	ConsoleUI.CriticalHit(attacker, target, damage);
};
