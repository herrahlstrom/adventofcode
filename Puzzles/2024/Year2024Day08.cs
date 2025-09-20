using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 8, "Resonant Collinearity")]
internal class Year2024Day08 : IPuzzle
{
    [Answer(240)]
    public object FirstPart()
    {
        var map = InputReader.ReadMap(this);
        HashSet<Point> counter = [];

        foreach (var p1 in GetPoints(map, new Point(0, 0)))
        {
            if (!IsAntenna(map[p1.X, p1.Y]))
                continue;

            foreach (var p2 in GetPoints(map, p1))
            {
                if (p2 == p1)
                    continue;

                if (map[p1.X, p1.Y] != map[p2.X, p2.Y])
                    continue;

                Point[] antinodes =
                [
                    ..GetAntinodes(map, p2, p2 - p1, 1),
                    ..GetAntinodes(map, p1, p1 - p2, 1)
                ];
                counter.AddRange(antinodes);
            }
        }

        return counter.Count;
    }

    [Answer(955)]
    public object SecondPart()
    {
        var map = InputReader.ReadMap(this);
        HashSet<Point> counter = [];

        foreach (var p1 in GetPoints(map, new Point(0, 0)))
        {
            if (!IsAntenna(map[p1.X, p1.Y]))
                continue;

            foreach (var p2 in GetPoints(map, p1))
            {
                if (p2 == p1)
                    continue;

                if (map[p1.X, p1.Y] != map[p2.X, p2.Y])
                    continue;

                Point[] antinodes =
                [
                    p1,
                    p2,
                    ..GetAntinodes(map, p2, p2 - p1, int.MaxValue),
                    ..GetAntinodes(map, p1, p1 - p2, int.MaxValue)
                ];
                counter.AddRange(antinodes);
            }
        }

        return counter.Count;
    }

    private static bool IsAntenna(char c)
    {
        return (c >= 48 && c <= 57) ||
               (c >= 65 && c <= 90) ||
               (c >= 97 && c <= 122);
    }

    private static bool OnMap(char[,] map, Point p)
    {
        return p.X >= 0 && p.X < map.GetLength(0) &&
               p.Y >= 0 && p.Y < map.GetLength(1);
    }

    private IEnumerable<Point> GetAntinodes(char[,] map, Point p, Point delta, int maxSteps)
    {
        var next = p + delta;
        var counter = 0;
        while (OnMap(map, next))

        {
            yield return next;
            next += delta;

            if (++counter >= maxSteps)
                break;
        }
    }

    private IEnumerable<Point> GetPoints(char[,] map, Point from)
    {
        for (var y = from.Y; y < map.GetLength(1); y++)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                if (y == from.Y && x < from.X)
                    continue;
                yield return new Point(x, y);
            }
        }
    }
}