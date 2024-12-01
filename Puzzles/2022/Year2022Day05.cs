using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 5, "Supply Stacks")]
internal partial class Year2022Day05 : IPuzzle
{
    public object FirstPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        Stack<char>[] stacks = ReadStacks(reader);

        foreach (var movement in ReadMovements(reader))
        {
            for (int i = 0; i < movement.Count; i++)
            {
                var crate = stacks[movement.From - 1].Pop();
                stacks[movement.To - 1].Push(crate);
            }
        }

        return CreateResult(stacks);
    }

    public object SecondPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        Stack<char>[] stacks = ReadStacks(reader);
        Stack<char> crane = new();

        foreach (var movement in ReadMovements(reader))
        {
            while (crane.Count < movement.Count)
            {
                var crate = stacks[movement.From - 1].Pop();
                crane.Push(crate);
            }

            while (crane.TryPop(out char crate))
            {
                stacks[movement.To - 1].Push(crate);
            }
        }

        return CreateResult(stacks);
    }

    private static object CreateResult(Stack<char>[] stacks)
    {
        char[] chars = new char[stacks.Length];
        for (int i = 0; i < stacks.Length; i++)
        {
            chars[i] = stacks[i].Peek();
        }
        return new string(chars);
    }

    [GeneratedRegex("^move (?<count>\\d+) from (?<from>\\d+) to (?<to>\\d+)$")]
    private static partial Regex MovementRegex();

    private static IEnumerable<Movement> ReadMovements(StreamReader reader)
    {
        Regex rx = MovementRegex();

        while (reader.ReadLine() is { Length: > 0 } line)
        {
            var lineMatch = rx.Match(line);
            yield return new Movement(
                int.Parse(lineMatch.Groups["count"].Value),
                int.Parse(lineMatch.Groups["from"].Value),
                int.Parse(lineMatch.Groups["to"].Value));
        }
    }

    private static Stack<char>[] ReadStacks(StreamReader reader)
    {
        List<List<char>> inputStacks = new();
        while (reader.ReadLine() is { Length: > 0 } line)
        {
            for (int i = 0, j = 0; j < line.Length; i++, j += 4)
            {
                while (inputStacks.Count <= i)
                {
                    inputStacks.Add(new List<char>());
                }
                if (line[j] == '[')
                {
                    inputStacks[i].Add(line[j + 1]);
                }
            }
        }

        var result = new Stack<char>[inputStacks.Count];
        for (int i = 0; i < inputStacks.Count; i++)
        {
            inputStacks[i].Reverse();
            result[i] = new Stack<char>(inputStacks[i]);
        }

        return result;
    }

    record struct Movement(int Count, int From, int To);
}
