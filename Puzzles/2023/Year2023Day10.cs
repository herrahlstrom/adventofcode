using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 10, "Pipe Maze")]
public class Year2023Day10 : IPuzzle
{
    private IList<Point>? _path = null;

    [Answer(7145)]
    public object FirstPart()
    {
        char[,] map = InputReader.ReadMap(this);

        var pathLength = GetPath(map).Count();
        return (int)Math.Floor(pathLength / 2m);
    }

    [Answer(445)]
    public object SecondPart()
    {
        char[,] map = InputReader.ReadMap(this);

        HashSet<Point> path = GetPath(map).ToHashSet();

        int count = 0;
        Dictionary<Point, int> cache = [];
        Point moveDirection = Point.Left + Point.Up;

        foreach(var p in GetPoints(map).Where(p => !path.Contains(p)))
        {
            int rayPathCounter = 0;

            var mp = p + moveDirection;
            while(InMap(map, mp))
            {
                if(cache.TryGetValue(mp, out int cachedValue))
                {
                    rayPathCounter += cachedValue;
                    break;
                }
                else
                {
                    if(path.Contains(mp) && map[mp.X, mp.Y] != 'L' && map[mp.X, mp.Y] != '7')
                    {
                        rayPathCounter++;
                    }
                    mp += moveDirection;
                }
            }

            if(rayPathCounter % 2 == 1)
            {
                count++;
            }

            cache[p] = rayPathCounter;
        }
        
        return count;
    }

    private static Point? CanGoDown(char[,] map, Point point)
    {
        if (InMap(map, point) && HasPipeDown(map, point))
        {
            var next = point.MoveDown();
            if (InMap(map, next) && HasPipeUp(map, next))
            {
                return next;
            }
        }
        return null;
    }
    private static Point? CanGoLeft(char[,] map, Point point)
    {
        if (InMap(map, point) && HasPipeLeft(map, point))
        {
            var next = point.MoveLeft();
            if (InMap(map, next) && HasPipeRight(map, next))
            {
                return next;
            }
        }
        return null;
    }
    private static Point? CanGoRight(char[,] map, Point point)
    {
        if (InMap(map, point) && HasPipeRight(map, point))
        {
            var next = point.MoveRight();
            if (InMap(map, next) && HasPipeLeft(map, next))
            {
                return next;
            }
        }
        return null;
    }
    private static Point? CanGoUp(char[,] map, Point point)
    {
        if (InMap(map, point) && HasPipeUp(map, point))
        {
            var next = point.MoveUp();
            if (InMap(map, next) && HasPipeDown(map, next))
            {
                return next;
            }
        }
        return null;
    }

    private static IEnumerable<Point> GetPoints(char[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                yield return new Point(x, y);
            }
        }
    }

    private static IEnumerable<Point> GetValidNaightbours(char[,] map, Point current)
    {
        if (CanGoLeft(map, current) is { } left)
            yield return left;

        if (CanGoRight(map, current) is { } right)
            yield return right;

        if (CanGoUp(map, current) is { } above)
            yield return above;

        if (CanGoDown(map, current) is { } below)
            yield return below;
    }

    private static bool HasPipeDown(char[,] map, Point point) => map[point.X, point.Y] is '|' or '7' or 'F' or 'S';
    private static bool HasPipeLeft(char[,] map, Point point) => map[point.X, point.Y] is '-' or 'J' or '7' or 'S';
    private static bool HasPipeRight(char[,] map, Point point) => map[point.X, point.Y] is '-' or 'L' or 'F' or 'S';
    private static bool HasPipeUp(char[,] map, Point point) => map[point.X, point.Y] is '|' or 'L' or 'J' or 'S';

    private static bool InMap(char[,] map, Point point) =>
        point.X >= 0 && point.X <= map.GetLength(0) &&
        point.Y >= 0 && point.Y <= map.GetLength(1);

    private IEnumerable<Point> CalculatePath(char[,] map)
    {
        Point start = GetPoints(map).First(p => map[p.X, p.Y] == 'S');

        yield return start;

        HashSet<Point> visited = [];
        Queue<Point> queue = new([start]);

        while (queue.TryDequeue(out var current))
        {
            foreach (var nb in GetValidNaightbours(map, current).Where(visited.Add))
            {
                queue.Enqueue(nb);
                yield return nb;
            }
        }
    }

    private IEnumerable<Point> GetPath(char[,] map)
    {
        return _path ??= CalculatePath(map).ToList();
    }
}
