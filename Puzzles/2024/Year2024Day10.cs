using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 10, "Hoof It")]
internal class Year2024Day10 : IPuzzle
{
    [Answer(496)]
    public object FirstPart()
    {
        var map = InputReader.ReadMap(this);
        return Calculate(map, false);
    }

    [Answer(1120)]
    public object SecondPart()
    {
        var map = InputReader.ReadMap(this);
        return Calculate(map, true);
    }

    private static int Calculate(char[,] map, bool distinctPaths)
    {
        var result = 0;

        List<Point> next = [];
        List<Point> buffer = [];
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(0); y++)
            {
                if (map[x, y] != '0')
                    continue;

                next.Clear();
                next.Add(new Point(x, y));

                for (var nextSymbol = '1'; nextSymbol <= '9'; nextSymbol++)
                {
                    buffer.Clear();
                    buffer.AddRange(
                        from current in next
                        from nextCandidate in GetAllDirections(current)
                        where map.OnMap(nextCandidate)
                        where map[nextCandidate.X, nextCandidate.Y] == nextSymbol
                        select nextCandidate);

                    next.Clear();

                    if (distinctPaths)
                        next.AddRange(buffer);
                    else
                        next.AddRange(buffer.Distinct());
                }

                result += next.Count;
            }
        }

        return result;
    }

    private static IEnumerable<Point> GetAllDirections(Point p)
    {
        yield return p.MoveLeft();
        yield return p.MoveUp();
        yield return p.MoveRight();
        yield return p.MoveDown();
    }
}