using System.Diagnostics;
using System.Reflection;
using ConsoleTables;

namespace AdventOfCode;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("""
                               _       _                 _            __    ____          _      
                              / \   __| |_   _____ _ __ | |_    ___  / _|  / ___|___   __| | ___ 
                             / _ \ / _` | \ / / _ \ '_ \| __|  / _ \| |_  | |   / _ \ / _` |/ _ \
                            / ___ \ (_| |\ V /  __/ | | | |_  | (_) |  _| | |__| (_) | (_| |  __/
                           /_/   \_\__,_| \_/ \___|_| |_|\__|  \___/|_|    \____\___/ \__,_|\___|
                          """);
        Console.WriteLine();

        var puzzles =
            from t in typeof(IPuzzle).Assembly.GetTypes()
            where t.IsClass && typeof(IPuzzle).IsAssignableFrom(t)
            let puzzle = t.GetCustomAttribute<PuzzleAttribute>()
            where puzzle != null
            select new { puzzle.Year, puzzle.Day, PuzzleType = t };

        puzzles = args.Length switch
        {
            0 => puzzles.Where(x => x.Year == DateTime.Now.Year),
            1 => puzzles.Where(x => x.Year == int.Parse(args[0])),
            2 => puzzles.Where(x => x.Year == int.Parse(args[0])).Where(x => x.Day == int.Parse(args[1])),
            _ => puzzles.Where(_ => false)
        };

        const int resultSize = 15;
        var resultTable = new ConsoleTable("YEAR", "DAY", "PUZZLE", "FIRST".PadLeft(resultSize), "SECOND".PadLeft(resultSize), "  ELAPSED");

        var start = Stopwatch.GetTimestamp();
        foreach (var puzzle in puzzles.Select(x => x.PuzzleType).Select(Activator.CreateInstance).OfType<IPuzzle>())
        {
            var puzzleType = puzzle.GetType();
            var puzzleAttribute = puzzleType.GetCustomAttribute<PuzzleAttribute>()!;

            var puzzleStart = Stopwatch.GetTimestamp();
            var firstResult = GetPuzzleValue(puzzleType, puzzle.FirstPart, nameof(puzzle.FirstPart));
            var secondResult = GetPuzzleValue(puzzleType, puzzle.SecondPart, nameof(puzzle.SecondPart));
            var puzzleElapsedTime = Stopwatch.GetElapsedTime(puzzleStart);

            resultTable.AddRow(
                $"{puzzleAttribute.Year}",
                $"{puzzleAttribute.Day,3}",
                $"{puzzleAttribute.Name}",
                $"{firstResult,resultSize}",
                $"{secondResult,resultSize}",
                $"{puzzleElapsedTime.TotalMilliseconds,7:N1} ms");
        }

        var elapsedTime = Stopwatch.GetElapsedTime(start);

        resultTable.Write();

        Console.WriteLine(" Elapsed: {0:N1} ms", elapsedTime.TotalMilliseconds);
    }

    private static object GetPuzzleValue(Type puzzleType, Func<object> method, string methodName)
    {
        var expectedAnswer = puzzleType.GetMethod(methodName)?.GetCustomAttribute<AnswerAttribute>()?.Value;

        try
        {
            var answer = method.Invoke();
            if (expectedAnswer != null && !expectedAnswer.Equals(answer))
                return $"{answer} (Expected: {expectedAnswer})";

            return answer;
        }
        catch (NotImplementedException)
        {
            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ex.GetType().Name;
        }
    }
}