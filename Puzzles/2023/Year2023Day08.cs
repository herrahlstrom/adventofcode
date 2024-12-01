using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2023;

using Map = Dictionary<string, (string, string)>;

[Puzzle(2023, 8, "Haunted Wasteland")]
public class Year2023Day08 : IPuzzle
{
    [Answer(12083)]
    public object FirstPart()
    {
        (string directions, Map map) = ReadInput();

        return WalkToEnd(map, directions, "AAA", str => str.Equals("ZZZ"));
    }

    [Answer(13385272668829L)]
    public object SecondPart()
    {
        (string directions, Map map) = ReadInput();

        var a = map.Keys.Where(str => str.EndsWith('A')).ToList();
        var b = a.Select(n => WalkToEnd(map, directions, n, str => str.EndsWith('Z'))).ToList();
        return LowestCommonMultiplier(b);

        static long LowestCommonMultiplier(List<int> numbers)
        {
            long step = numbers[0];
            long multiplier = step;
            for (int i = 1; i < numbers.Count; i++)
            {
                while (multiplier % numbers[i] > 0)
                {
                    multiplier += step;
                }
                step = multiplier;
            }
            return multiplier;
        }
    }

    private static int WalkToEnd(Map map, string directions, string start, Predicate<string> isAtEnd)
    {
        string current = start;
        int jumps = 0;

        for (int i = 0; ; i++)
        {
            if (i >= directions.Length)
                i = 0;

            current = directions[i] == 'L' ? map[current].Item1 : map[current].Item2;
            jumps++;
            if (isAtEnd.Invoke(current))
            {
                return jumps;
            }
        }
    }

    private (string directions, Map map) ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);
        string directions = reader.ReadLine()!;
        reader.ReadLine();

        Map map = new(800);
        while (reader.ReadLine() is { } line)
        {
            map.Add(line[0..3], (line[7..10], line[12..15]));
        }

        return (directions, map);
    }
}
