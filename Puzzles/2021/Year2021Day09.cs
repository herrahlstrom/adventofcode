using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 9, "Smoke Basin")]
internal class Year2021Day09 : IPuzzle
{
    private static readonly Point Up = new(0, -1);
    private static readonly Point Right = new(1, 0);
    private static readonly Point Down = new(0, 1);
    private static readonly Point Left = new(-1, 0);
    private Point _end;
    private IList<int> _values = Array.Empty<int>();

    public object FirstPart()
    {
        ReadInput();

        long risk = 0;

        for (int i = 0; i < _values.Count; i++)
        {
            Point p = GetPoint(i);

            if (p.X > 0 && _values[GetIndex(p + Left)] <= _values[i])
            {
                continue;
            }

            if (p.Y > 0 && _values[GetIndex(p + Up)] <= _values[i])
            {
                continue;
            }

            if (p.X < _end.X && _values[GetIndex(p + Right)] <= _values[i])
            {
                continue;
            }

            if (p.Y < _end.Y && _values[GetIndex(p + Down)] <= _values[i])
            {
                continue;
            }

            risk += _values[i] + 1;
        }

        return risk;
    }

    public void ReadInput()
    {
        string[] lines = InputReader.ReadAllLines(this);
        _end = new Point(lines[0].Length - 1, lines.Length - 1);

        _values = string.Join("", lines).Select(c => c - 48).ToArray();
    }

    public object SecondPart()
    {
        var basinCells = new HashSet<Point>();
        var basins = new List<ICollection<Point>>();

        for (int i = 0; i < _values.Count; i++)
        {
            if (_values[i] == 9)
            {
                continue;
            }

            Point p = GetPoint(i);
            if (basinCells.Contains(p))
            {
                continue;
            }

            var basin = GetBasin(p).ToList();
            basinCells.AddRange(basin);

            basins.Add(basin);
        }

        var largestBasins = basins.OrderByDescending(x => x.Count).Take(3);
        return largestBasins.Aggregate((long)1, (a, basin) => a * basin.Count);
    }

    private IEnumerable<Point> GetBasin(Point start)
    {
        var pos = new HashSet<Point>();
        var queue = new Queue<Point>();

        queue.Enqueue(start);

        while (queue.TryDequeue(out Point p))
        {
            if (pos.Add(p))
            {
                yield return p;
            }
            else
            {
                continue;
            }

            if (p.X > 0)
            {
                Point l = p + Left;
                if (_values[GetIndex(l)] < 9)
                {
                    queue.Enqueue(l);
                }
            }

            if (p.Y > 0)
            {
                Point u = p + Up;
                if (_values[GetIndex(u)] < 9)
                {
                    queue.Enqueue(u);
                }
            }

            if (p.X < _end.X)
            {
                Point r = p + Right;
                if (_values[GetIndex(r)] < 9)
                {
                    queue.Enqueue(r);
                }
            }

            if (p.Y < _end.Y)
            {
                Point d = p + Down;
                if (_values[GetIndex(d)] < 9)
                {
                    queue.Enqueue(d);
                }
            }
        }
    }

    private int GetIndex(Point p) => p.Y * (_end.X + 1) + p.X;

    private Point GetPoint(int n) => new(n % (_end.X + 1), n / (_end.X + 1));
}