using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 14, "Regolith Reservoir")]
public class Year2022Day14 : IPuzzle
{
    public object FirstPart()
    {
        var map = ReadMap(out int maxY);
        int unitOfSand = 0;

        while (true)
        {
            Point p = new(500, 0);
            while (true)
            {
                if (p.Y > maxY)
                {
                    return unitOfSand;
                }

                if (CanFall(map, p, out Point next))
                {
                    p = next;
                }
                else
                {
                    map.Add(p);
                    break;
                }
            }
            unitOfSand++;
        }
    }

    public object SecondPart()
    {
        var map = ReadMap(out int maxY);
        int unitOfSand = 0;
        int floor = maxY + 2;

        while (true)
        {
            Point p = new(500, 0);
            while (true)
            {
                if (CanFall(map, p, out Point next) && next.Y < floor)
                {
                    p = next;
                }
                else if (p.Y == 0)
                {
                    return unitOfSand + 1;
                }
                else
                {
                    map.Add(p);
                    break;
                }
            }
            unitOfSand++;
        }
    }

    static IEnumerable<Point> GetPointsOnVector(Point a, Point b)
    {
        if (a == b)
        {
            yield return a;
            yield break;
        }

        int dx = Math.Sign(b.X - a.X);
        int dy = Math.Sign(b.Y - a.Y);
        var p = a;

        do
        {
            yield return p;
            p = new Point(p.X + dx, p.Y + dy);
        } while (p != b);
        yield return b;
    }

    bool CanFall(HashSet<Point> map, Point p, out Point next)
    {
        var temp = p with { Y = p.Y + 1 };
        if (!map.Contains(temp))
        {
            next = temp;
            return true;
        }

        temp = p with { X = p.X - 1, Y = p.Y + 1 };
        if (!map.Contains(temp))
        {
            next = temp;
            return true;
        }

        temp = p with { X = p.X + 1, Y = p.Y + 1 };
        if (!map.Contains(temp))
        {
            next = temp;
            return true;
        }

        next = new(0, 0);
        return false;
    }

    HashSet<Point> ReadMap(out int maxY)
    {
        var map = new HashSet<Point>(1000);
        maxY = 0;
        foreach (var path in ReadPaths())
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                foreach (var p in GetPointsOnVector(path[i], path[i + 1]))
                {
                    map.Add(p);

                    if (p.Y > maxY)
                    {
                        maxY = p.Y;
                    }
                }
            }
        }
        return map;
    }

    IEnumerable<Point[]> ReadPaths()
    {
        foreach (var line in InputReader.ReadLines(this))
        {
            yield return line.Split(" -> ")
                .Select(x => x.Split(','))
                .Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])))
                .ToArray();
        }
    }

    record Point(int X, int Y);
}
