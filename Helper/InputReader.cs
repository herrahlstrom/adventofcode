using System.Reflection;

namespace AdventOfCode.Helper;

internal static class InputReader
{
    public static StreamReader OpenStreamReader<TPuzzle>(TPuzzle puzzle)
    {
        string path = GetInputPath<TPuzzle>();

        var file = File.OpenRead(path);
        return new StreamReader(file);
    }

    public static string[] ReadAllLines<TPuzzle>(TPuzzle puzzle)
    {
        string path = GetInputPath<TPuzzle>();
        return File.ReadAllLines(path);
    }

    public static string ReadAllText<TPuzzle>(TPuzzle puzzle)
    {
        string path = GetInputPath<TPuzzle>();
        return File.ReadAllText(path);
    }

    public static IEnumerable<string> ReadLines<TPuzzle>(TPuzzle puzzle)
    {
        string path = GetInputPath<TPuzzle>();
        return File.ReadLines(path);
    }

    private static string GetInputPath<TPuzzle>()
    {
        PuzzleAttribute puzzleAttribute = typeof(TPuzzle).GetCustomAttribute<PuzzleAttribute>()!;
        string path = $"input/{puzzleAttribute.Year}/{puzzleAttribute.Day:00}.txt";
        return path;
    }
}
