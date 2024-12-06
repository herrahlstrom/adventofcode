using System.Diagnostics;
using AdventOfCode.Helper;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 6, "Guard Gallivant")]
internal class Year2024Day06 : IPuzzle
{
    private const char UpSign = '^';
    private const char RightSign = '>';
    private const char LeftSign = '<';
    private const char DownSign = 'v';
    private static readonly Point Up = new Point(0, -1);
    private static readonly Point Down = new Point(0, 1);
    private static readonly Point Left = new Point(-1, 0);
    private static readonly Point Right = new Point(1, 0);

    [Answer(4776)]
    public object FirstPart()
    {
        char[,] map = InputReader.ReadMap(this);
        Point guard = FindGuard(map);

        HashSet<Point> visited = [];

        while (true)
        {
            Point next = guard + GetDirection(map, guard);
            if (!OnMap(map, next)) break;

            if (map[next.X, next.Y] == '#')
            {
                map[guard.X, guard.Y] = TurnRight(map, guard);
                continue;
            }

            map[next.X, next.Y] = map[guard.X, guard.Y];

            guard = next;
            visited.Add(next);
        }

        return visited.Count;
    }

    [Answer(1586)]
    public object SecondPart()
    {
        char[,] map = InputReader.ReadMap(this);
        Point guardOrig = FindGuard(map);

        int result = 0;
        for (int obstX = 0; obstX < map.GetLength(0); obstX++)
        {
            for (int obstY = 0; obstY < map.GetLength(1); obstY++)
            {
                if (map[obstX, obstY] == '#' || (guardOrig.X == obstX && guardOrig.Y == obstY))
                    continue;

                map[guardOrig.X, guardOrig.Y] = UpSign;
                Point guard = guardOrig;
                HashSet<Vector> visited = [];
                bool inLoop = false;

                while (true)
                {
                    Point direction = GetDirection(map, guard);
                    Point next = guard + direction;
                    if (!OnMap(map, next)) break;

                    if (map[next.X, next.Y] == '#' || (next.X == obstX && next.Y == obstY))
                    {
                        map[guard.X, guard.Y] = TurnRight(map, guard);
                        continue;
                    }

                    map[next.X, next.Y] = map[guard.X, guard.Y];

                    guard = next;

                    var v = new Vector(next, direction);

                    if (!visited.Add(v))
                    {
                        inLoop = true;
                        break;
                    }
                }

                if (inLoop)
                    result++;
            }
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

    private Point GetDirection(char[,] map, Point guard)
    {
        return map[guard.X, guard.Y] switch
        {
            UpSign => Up,
            RightSign => Right,
            LeftSign => Left,
            DownSign => Down,
            _ => throw new UnreachableException($"Invalid direction of guard; ({guard.X},{guard.Y}) '{map[guard.X, guard.Y]}'.")
        };
    }

    private static bool OnMap(char[,] map, Point point)
    {
        return point.X >= 0 &&
               point.X < map.GetLength(0) &&
               point.Y >= 0 &&
               point.Y < map.GetLength(1);
    }

    private char TurnRight(char[,] map, Point current)
    {
        return map[current.X, current.Y] switch
        {
            UpSign => RightSign,
            RightSign => DownSign,
            LeftSign => UpSign,
            DownSign => LeftSign,
            _ => throw new UnreachableException("Invalid direction of guard.")
        };
    }

    private record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }

    private record struct Vector(Point Point, Point Direction);
}