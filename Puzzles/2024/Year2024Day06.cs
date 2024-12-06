using System.Diagnostics;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 6, "Guard Gallivant")]
internal class Year2024Day06 : IPuzzle
{
    private static readonly Point[] Directions = [new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0)];

    [Answer(4776)]
    public object FirstPart()
    {
        char[,] map = InputReader.ReadMap(this);
        Point guard = FindGuard(map);

        return GetPathOut(map, guard).Distinct().Count();
    }

    [Answer(1586)]
    public object SecondPart()
    {
        char[,] map = InputReader.ReadMap(this);
        Point guardOrig = FindGuard(map);

        var candidates = GetPathOut(map, guardOrig).ToHashSet();
        candidates.Remove(guardOrig);

        HashSet<(Point, Point)> visited = [];

        int result = 0;
        foreach (Point p in candidates)
        {
            map[p.X, p.Y] = '#';

            Point guard = guardOrig;
            int direction = 0;

            bool inLoop = false;

            visited.Clear();
            while (true)
            {
                Point next = guard + Directions[direction];
                if (!OnMap(map, next)) break;

                if (map[next.X, next.Y] == '#')
                {
                    TurnRight(ref direction);
                    continue;
                }

                guard = next;

                if (visited.Add((next, Directions[direction])))
                {
                    continue;
                }

                inLoop = true;
                break;
            }

            if (inLoop)
                result++;

            map[p.X, p.Y] = '.';
        }

        return result;
    }

    private Point FindGuard(char[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == '^')
                {
                    return new Point(x, y);
                }
            }
        }

        throw new UnreachableException("The guard is not reachable.");
    }

    private IEnumerable<Point> GetPathOut(char[,] map, Point guard)
    {
        int direction = 0;

        while (true)
        {
            Point next = guard + Directions[direction];
            if (!OnMap(map, next)) break;

            if (map[next.X, next.Y] == '#')
            {
                TurnRight(ref direction);
                continue;
            }

            guard = next;
            yield return next;
        }
    }

    private static bool OnMap(char[,] map, Point point)
    {
        return point.X >= 0 &&
               point.X < map.GetLength(0) &&
               point.Y >= 0 &&
               point.Y < map.GetLength(1);
    }

    private static void TurnRight(ref int direction)
    {
        direction++;
        if (direction >= Directions.Length)
            direction = 0;
    }

    private record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }
}