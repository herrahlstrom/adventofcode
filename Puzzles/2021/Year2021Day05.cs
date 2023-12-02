using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 5, "Hydrothermal Venture")]
internal class Year2021Day05 : IPuzzle
{
    private readonly List<Tuple<Point, Point>> _lines = new();
    private Point _max = new(0, 0);

    public object FirstPart()
    {
        ReadInput();

        int[] hits = new int[GetIndex(_max) + 1];

        foreach ((Point a, Point b) in _lines)
        {
            Point d = b - a;

            Point step;
            if (d.X == 0)
            {
                step = new Point(0, Math.Sign(d.Y));
            }
            else if (d.Y == 0)
            {
                step = new Point(Math.Sign(d.X), 0);
            }
            else
            {
                continue;
            }

            foreach (Point p in GetPoints(a, b, step))
            {
                hits[GetIndex(p)]++;
            }
        }


        return hits.Count(x => x >= 2);
    }

    public void ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);
        var rx = new Regex(@"^(\d+),(\d+) -> (\d+),(\d+)$");

        int maxX = int.MinValue;
        int maxY = int.MinValue;

        while (!reader.EndOfStream && reader.ReadLine() is { } line)
        {
            Match rxMatch = rx.Match(line);

            var a = new Point(int.Parse(rxMatch.Groups[1].Value), int.Parse(rxMatch.Groups[2].Value));
            var b = new Point(int.Parse(rxMatch.Groups[3].Value), int.Parse(rxMatch.Groups[4].Value));
            _lines.Add(Tuple.Create(a, b));

            maxX = Math.Max(maxX, a.X);
            maxX = Math.Max(maxX, b.X);
            maxY = Math.Max(maxY, a.Y);
            maxY = Math.Max(maxY, b.Y);
        }

        _max = new Point(maxX, maxY);
    }

    public object SecondPart()
    {
        int[] hits = new int[GetIndex(_max) + 1];

        foreach ((Point a, Point b) in _lines)
        {
            Point d = b - a;

            Point step;
            if (d.X == 0)
            {
                step = new Point(0, Math.Sign(d.Y));
            }
            else if (d.Y == 0)
            {
                step = new Point(Math.Sign(d.X), 0);
            }
            else
            {
                step = new Point(Math.Sign(d.X), Math.Sign(d.Y));
            }

            foreach (Point p in GetPoints(a, b, step))
            {
                hits[GetIndex(p)]++;
            }
        }


        return hits.Count(x => x >= 2);
    }

    private static IEnumerable<Point> GetPoints(Point a, Point b, Point step)
    {
        Point p = a;
        yield return p;
        while (p != b)
        {
            p += step;
            yield return p;
        }
    }

    private int GetIndex(Point p)
    {
        return p.Y * (_max.X + 1) + p.X;
    }
}