using System;
using System.Diagnostics;
using System.Reflection;
using AdventOfCode;
using AdventOfCode.Helper;
using ConsoleTables;

var resultTable = new ConsoleTable("   PUZZLE", "      FIRST", "     SECOND", "  ELAPSED");

var arguments = new CommandLine.Parser().ParseArguments<Arguments>(args);
foreach(var argError in arguments.Errors)
{
    throw new ArgumentException(argError.ToString());
}

foreach (IPuzzle day in GetPuzzles(arguments.Value))
{
    var start = Stopwatch.GetTimestamp();

    object firstResult;
    try
    {
        firstResult = day.FirstPart();
    }
    catch (NotImplementedException)
    { firstResult = ""; }

    object secondResult;
    try
    {
        secondResult = day.SecondPart();
    }
    catch (NotImplementedException)
    { secondResult = ""; }

    var elapsedTime = Stopwatch.GetElapsedTime(start);
    
    PuzzleAttribute puzzleAttribute = day.GetType().GetCustomAttribute<PuzzleAttribute>()!;

    resultTable.AddRow(
        $"{puzzleAttribute.Day,2} {puzzleAttribute.Name}",
        $"{firstResult,11}",
        $"{secondResult,11}",
        $"{elapsedTime.TotalMilliseconds,6:N1} ms");
}

resultTable.Write();

static IEnumerable<IPuzzle> GetPuzzles(Arguments arguments)
{
    ArgumentNullException.ThrowIfNull(arguments);

    var puzzles = (from t in typeof(IPuzzle).Assembly.GetTypes()
                   where t.IsClass && typeof(IPuzzle).IsAssignableFrom(t)
                   let puzzle = t.GetCustomAttribute<PuzzleAttribute>()
                   where puzzle != null
                   select new { puzzle.Year, puzzle.Day, PuzzleType = t }).ToList();

    int year = arguments.Year ?? puzzles.Max(x => x.Year);

    return puzzles.Where(x => x.Year == year)
                  .Where(x => arguments.Day == null || x.Day == arguments.Day)
                  .Select(x => x.PuzzleType)
                  .Select(Activator.CreateInstance)
                  .OfType<IPuzzle>();
}
