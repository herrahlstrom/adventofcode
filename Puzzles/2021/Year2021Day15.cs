using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 15, "Chiton")]
internal class Year2021Day15 : IPuzzle
{
    private int[,] _map = new int[0, 0];

    public object FirstPart()
    {
        ReadInput();

        int[,] map = new int[_map.GetLength(0), _map.GetLength(1)];
        for (int n = 0; n < _map.GetLength(0); n++)
        {
            for (int m = 0; m < _map.GetLength(1); m++)
            {
                map[n, m] = _map[n, m];
            }
        }

        return FindBestScore(map);
    }

    public void ReadInput()
    {
        string[] lines = InputReader.ReadAllLines(this);
        _map = new int[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                _map[x, y] = line[x] - 48;
            }
        }
    }

    public object SecondPart()
    {
        int[,] map = new int[_map.GetLength(0) * 5, _map.GetLength(1) * 5];
        for (int x = 0; x < _map.GetLength(0); x++)
        {
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                for (int xFactor = 0; xFactor < 5; xFactor++)
                {
                    for (int yFactor = 0; yFactor < 5; yFactor++)
                    {
                        int mX = x + _map.GetLength(0) * xFactor;
                        int mY = y + _map.GetLength(1) * yFactor;
                        map[mX, mY] = _map[x, y] + xFactor + yFactor;
                        while (map[mX, mY] > 9)
                        {
                            map[mX, mY] -= 9;
                        }
                    }
                }
            }
        }

        return FindBestScore(map);
    }

    private static object FindBestScore(int[,] map)
    {
        int[,] score = new int[map.GetLength(0), map.GetLength(1)];
        for (int n = 0; n < map.GetLength(0); n++)
        {
            for (int m = 0; m < map.GetLength(1); m++)
            {
                score[n, m] = int.MaxValue;
            }
        }

        var start = new Point(0, 0);
        var target = new Point(map.GetLength(0) - 1, map.GetLength(1) - 1);

        score[start.X, start.Y] = 0;

        Queue<Point> queue = new();
        queue.Enqueue(new Point(0, 0));

        int bestScore = int.MaxValue;

        while (queue.TryDequeue(out Point p))
        {
            var neighbours = GetNeighbours(p)
                .Where(next => next.X >= 0 && next.X < map.GetLength(0))
                .Where(next => next.Y >= 0 && next.Y < map.GetLength(1))
                .ToList();
            foreach (Point neighbour in neighbours)
            {
                int pathRisk = score[p.X, p.Y] + map[neighbour.X, neighbour.Y];
                if (pathRisk >= score[neighbour.X, neighbour.Y])
                {
                    continue;
                }

                if (pathRisk >= bestScore)
                {
                    continue;
                }

                score[neighbour.X, neighbour.Y] = pathRisk;
                if (neighbour == target)
                {
                    if (pathRisk < bestScore)
                    {
                        bestScore = pathRisk;
                    }
                }
                else
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        return bestScore;
    }

    private static IEnumerable<Point> GetNeighbours(Point p)
    {
        yield return p + Point.Up;
        yield return p + Point.Right;
        yield return p + Point.Down;
        yield return p + Point.Left;
    }
}