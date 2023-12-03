using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using ConsoleTables;

namespace AdventOfCode;

public class Program
{
    public static void Main(string[] args)
    {
        Arguments arguments = GetArguments(args);

        ConcurrentBag<PuzzleResult> result = [];

        var start = Stopwatch.GetTimestamp();
        Parallel.ForEach(
            GetPuzzles(arguments),
            new ParallelOptions() { MaxDegreeOfParallelism = 3 },
            day => result.Add(SolvePuzzle(day)));
        var elapsedTime = Stopwatch.GetElapsedTime(start);

        PrintResultTable(result);

        Console.WriteLine(" Elapsed: {0:N1} ms", elapsedTime.TotalMilliseconds);
    }

    private static Arguments GetArguments(string[] args)
    {
        var arguments = new CommandLine.Parser().ParseArguments<Arguments>(args);

        if (arguments.Errors.FirstOrDefault() is { } argParseError)
        {
            throw new ArgumentException(argParseError.ToString());
        }

        return arguments.Value;
    }

    private static IEnumerable<IPuzzle> GetPuzzles(Arguments arguments)
    {
        var puzzles = (from t in typeof(IPuzzle).Assembly.GetTypes()
                       where t.IsClass && typeof(IPuzzle).IsAssignableFrom(t)
                       let puzzle = t.GetCustomAttribute<PuzzleAttribute>()
                       where puzzle != null
                       select new { puzzle.Year, puzzle.Day, PuzzleType = t }).ToList();

        int year;
        int? day;
        if(arguments.Latest)
        {
            year = puzzles.Max(x => x.Year);
            day = puzzles.Where(x => x.Year == year).Max(x => x.Day);
        }
        else
        {
            year = arguments.Year ?? puzzles.Max(x => x.Year);
            day = arguments.Day;
        }
    
        return puzzles.Where(x => x.Year == year)
                      .Where(x => day == null || x.Day == day)
                      .Select(x => x.PuzzleType)
                      .Select(Activator.CreateInstance)
                      .OfType<IPuzzle>();
    }

    private static void PrintResultTable(ConcurrentBag<PuzzleResult> result)
    {
        const int ResultSize = 15;

        var resultTable = new ConsoleTable("   PUZZLE", "FIRST".PadLeft(ResultSize), "SECOND".PadLeft(ResultSize), "  ELAPSED");
        foreach (var puzzleResult in result.OrderBy(x => x.Year).ThenBy(x => x.Day))
        {
            resultTable.AddRow(
                $"{puzzleResult.Day,2} {puzzleResult.Name}",
                $"{puzzleResult.FirstResult,ResultSize}",
                $"{puzzleResult.SecondResult,ResultSize}",
                $"{puzzleResult.Elapsed.TotalMilliseconds,7:N1} ms");
        }
        resultTable.Write();
    }

    private static readonly SemaphoreSlim s_singleRunSemaphore = new(1);

    private static PuzzleResult SolvePuzzle(IPuzzle day)
    {
        PuzzleAttribute puzzleAttribute = day.GetType().GetCustomAttribute<PuzzleAttribute>()!;

        bool locked = false;
        if(puzzleAttribute.PrintToConsole)
        {
            s_singleRunSemaphore.Wait();
            locked = true;
        }

        try
        {
            var puzzleStart = Stopwatch.GetTimestamp();
            object firstResult = GetValue(day.FirstPart);
            object secondResult = GetValue(day.SecondPart);
            TimeSpan puzzleElapsedTime = Stopwatch.GetElapsedTime(puzzleStart);

            return new PuzzleResult(puzzleAttribute.Year, puzzleAttribute.Day, puzzleAttribute.Name, firstResult, secondResult, puzzleElapsedTime);
        }
        finally
        {
            if(locked)
            {
                s_singleRunSemaphore.Release();
            }
        }

        static object GetValue(Func<object> method)
        {
            try
            {
                return method.Invoke();
            }
            catch (NotImplementedException)
            {
                return "";
            }
            catch(Exception ex)
            {
                return ex.GetType().Name;
            }
        }
    }

    private record PuzzleResult(int Year, int Day, string Name, object? FirstResult, object? SecondResult, TimeSpan Elapsed);
}