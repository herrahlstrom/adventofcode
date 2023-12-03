using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2023, 3, "Gear Ratios")]
public class Year2023Day03 : IPuzzle
{
    public object FirstPart()
    {
        string[] lines = InputReader.ReadAllLines(this);

        return Search(lines, c => c != '.' && !char.IsNumber(c), p => GetNeighbourValues(lines, p).Sum()).Sum();
    }

    public object SecondPart()
    {
        string[] lines = InputReader.ReadAllLines(this);

        return Search(lines, c => c == '*', p => GetNeighbourValues(lines, p).ToList())
               .Where(x => x.Count == 2)
               .Sum(x => x[0] * x[1]);
    }

    private static IEnumerable<TResult> Search<TResult>(string[] lines, Predicate<char> predicate, Func<Point, TResult> onSymbol)
    {
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (predicate(lines[y][x]))
                {
                    yield return onSymbol.Invoke(new Point(x, y));
                }
            }
        }
    }

    private IEnumerable<int> GetNeighbourValues(string[] lines, Point p)
    {
        int width = lines[0].Length;
        HashSet<int> valueStarts = [];

        IEnumerable<Point> nbPoints = [
            p.MoveUp().MoveLeft(),
            p.MoveUp(),
            p.MoveUp().MoveRight(),
            p.MoveLeft(),
            p.MoveRight(),
            p.MoveDown().MoveLeft(),
            p.MoveDown(),
            p.MoveDown().MoveRight()];

        foreach (var nbPoint in nbPoints)
        {
            if (nbPoint.Y < 0 || nbPoint.Y > lines.Length - 1 || nbPoint.X < 0 || nbPoint.X > width - 1)
            {
                continue;
            }

            if (!char.IsNumber(lines[nbPoint.Y][nbPoint.X]))
            {
                continue;
            }

            int a = nbPoint.X;
            while (a > 0 && char.IsNumber(lines[nbPoint.Y][a - 1]))
            {
                a--;
            }

            int b = nbPoint.X;
            while (b < width && char.IsNumber(lines[nbPoint.Y][b]))
            {
                b++;
            }

            if (valueStarts.Add(nbPoint.Y * width + a))
            {
                yield return int.Parse(lines[nbPoint.Y][a..b]);
            }
        }
    }
}
