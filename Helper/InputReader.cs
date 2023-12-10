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

    public static char[,] ReadMap<TPuzzle>(TPuzzle puzzle)
    {
        string path = GetInputPath<TPuzzle>();
        var lines = File.ReadAllLines(path);

        var map = new char[lines[0].Length, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[x, y] = lines[y][x];
            }
        }
        return map;
    }

    private static string GetInputPath<TPuzzle>()
    {
        PuzzleAttribute puzzleAttribute = typeof(TPuzzle).GetCustomAttribute<PuzzleAttribute>()!;
        string path = $"input/{puzzleAttribute.Year}/{puzzleAttribute.Day:00}.txt";
        return path;
    }
}
