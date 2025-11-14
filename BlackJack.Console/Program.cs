using BlackJack.Game;

class Program
{
    static void Main()
    {
        var engine = new GameEngine();
        var initial = engine.StartRound();

        Console.WriteLine($"Initial: {initial}");

        var hitOutcome = engine.PlayerHit();

        Console.WriteLine($"After hit: {hitOutcome}");

        var finalOutcome = engine.PlayerStand();

        Console.WriteLine($"Final outcome: {finalOutcome}");

    }
}