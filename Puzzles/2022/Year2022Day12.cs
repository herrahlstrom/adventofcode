using System.Diagnostics;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 12, "Hill Climbing Algorithm")]
internal class Year2022Day12 : IPuzzle
{
    delegate bool CanMove(char[,] map, Point point, Point target);

    public object FirstPart()
    {
        char[,] map = ReadMap(out Point start, out Point target);

        Point[,][] trail = new Point[map.GetLength(0), map.GetLength(1)][];
        trail[start.X, start.Y] = Array.Empty<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(start);

        var directions = new[] { TryGoRight, TryGoDown, TryGoLeft, TryGoUp };

        while(queue.TryDequeue(out Point? p) && p is { } current)
        {
            if(current == target)
            {
                return trail[target.X, target.Y].Length;
            }

            foreach(var dir in directions)
            {
                if(dir.Invoke(map, current, out Point next, CanMoveForward) && (trail[next.X, next.Y] is null))
                {
                    trail[next.X, next.Y] = trail[current.X, current.Y].Append(next).ToArray();
                    queue.Enqueue(next);
                }
            }
        }

        throw new UnreachableException();
    }

    public object SecondPart()
    {
        char[,] map = ReadMap(out _, out Point target);

        Point[,][] trail = new Point[map.GetLength(0), map.GetLength(1)][];
        trail[target.X, target.Y] = Array.Empty<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(target);

        var directions = new[] { TryGoRight, TryGoDown, TryGoLeft, TryGoUp };

        // Start from end and get length to all reachable positions
        while(queue.TryDequeue(out Point? p) && p is { } current)
        {
            foreach(var dir in directions)
            {
                if(dir.Invoke(map, current, out Point next, CanMoveBackward) && (trail[next.X, next.Y] is null))
                {
                    trail[next.X, next.Y] = trail[current.X, current.Y].Append(next).ToArray();
                    queue.Enqueue(next);
                }
            }
        }

        int bestPathLength = int.MaxValue;
        for(int x = 0; x < map.GetLength(0); x++)
        {
            for(int y = 0; y < map.GetLength(1); y++)
            {
                if(map[x, y] == 'a')
                {
                    if(trail[x, y] is not Point[] path)
                    {
                        continue;
                    }
                    var pathLength = path.Length;
                    if(pathLength < bestPathLength)
                    {
                        bestPathLength = pathLength;
                    }
                }
            }
        }
        return bestPathLength;
    }

    private static bool CanMoveBackward(char[,] map, Point current, Point next)
    {
        return CanMoveForward(map, next, current);
    }

    private static bool CanMoveForward(char[,] map, Point current, Point next)
    {
        var a = map[current.X, current.Y];
        if(a == 'S')
        {
            a = 'a';
        }
        var b = map[next.X, next.Y];
        if(b == 'E')
        {
            b = 'z';
        }

        return a >= b - 1;
    }

    private static bool InMap(char[,] map, Point target)
    {
        return
            target.X >= 0 &&
            target.X < map.GetLength(0) &&
            target.Y >= 0 &&
            target.Y < map.GetLength(1);
    }

    private static bool TryGoDown(char[,] map, Point point, out Point below, CanMove canMoveDelegate)
    {
        below = point with { Y = point.Y + 1 };
        return InMap(map, below) && canMoveDelegate.Invoke(map, point, below);
    }

    private static bool TryGoLeft(char[,] map, Point point, out Point left, CanMove canMoveDelegate)
    {
        left = point with { X = point.X - 1 };
        return InMap(map, left) && canMoveDelegate.Invoke(map, point, left);
    }

    private static bool TryGoRight(char[,] map, Point point, out Point right, CanMove canMoveDelegate)
    {
        right = point with { X = point.X + 1 };
        return InMap(map, right) && canMoveDelegate.Invoke(map, point, right);
    }

    private static bool TryGoUp(char[,] map, Point point, out Point above, CanMove canMoveDelegate)
    {
        above = point with { Y = point.Y - 1 };
        return InMap(map, above) && canMoveDelegate.Invoke(map, point, above);
    }

    private char[,] ReadMap(out Point start, out Point target)
    {
        start = Point.Empty;
        target = Point.Empty;

        var lines = InputReader.ReadAllLines(this);
        var map = new char[lines[0].Length, lines.Length];

        for(int y = 0; y < map.GetLength(1); y++)
        {
            for(int x = 0; x < map.GetLength(0); x++)
            {
                var c = lines[y][x];
                if(c == 'S')
                {
                    start = new Point(x, y);
                }
                else if(c == 'E')
                {
                    target = new Point(x, y);
                }
                map[x, y] = lines[y][x];
            }
        }

        return map;
    }

    record Point(int X, int Y)
    {
        public static Point Empty { get; } = new Point(0, 0);
    }
}
