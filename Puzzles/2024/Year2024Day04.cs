using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 4, "Ceres Search")]
internal class Year2024Day04 : IPuzzle
{
    [Answer(2662)]
    public object FirstPart()
    {
        char[,] map = InputReader.ReadMap(this);
        int counter = 0;

        List<Point> directions = [new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(-1, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1), new Point(1, 1)];
        const string match = "XMAS";

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != match[0])
                    continue;

                foreach (Point direction in directions)
                {
                    if (FindMatch(map, match, new Point(x, y), direction))
                        counter++;
                }
            }
        }

        return counter;
    }

    public object SecondPart()
    {
        char[,] map = InputReader.ReadMap(this);
        int counter = 0;
        const string match = "MAS";
        List<Point> directions = [new Point(-1, -1), new Point(1, -1), new Point(-1, 1), new Point(1, 1)];

        HashSet<Point> candidated = [];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] != match[0])
                    continue;

                foreach (Point direction in directions)
                {
                    var start = new Point(x, y);
                    if (FindMatch(map, match, start, direction))
                    {
                        var origo = start + direction;
                        if (!candidated.Add(origo))
                        {
                            counter++;
                        }
                    }
                }
            }
        }

        return counter;
    }

    private bool FindMatch(char[,] map, string match, Point start, Point direction)
    {
        Point current = start;
        foreach (char t in match)
        {
            if (current.X < 0 || current.X >= map.GetLength(0) ||
                current.Y < 0 || current.Y >= map.GetLength(1) ||
                map[current.X, current.Y] != t)
            {
                return false;
            }

            current += direction;
        }

        return true;
    }

    private record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }
}