using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 8, "Treetop Tree House")]
internal class Year2022Day08 : IPuzzle
{
    public object FirstPart()
    {
        char[,] map = GetMap();

        int result = 0;

        // Top + Bottom
        result += map.GetLength(0) * 2;

        // Left + Right (without corners)
        result += map.GetLength(1) * 2 - 4;

        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (IsVisible(0, x, (int i) => map[i, y]) ||
                    IsVisible(x + 1, map.GetLength(0), (int i) => map[i, y]) ||
                    IsVisible(0, y, (int i) => map[x, i]) ||
                    IsVisible(y + 1, map.GetLength(1), (int i) => map[x, i]))
                {
                    result++;
                }

                bool IsVisible(int start, int end, Func<int, char> v)
                {
                    for (int i = start; i < end; i++)
                    {
                        if (v(i) >= map[x, y])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        return result;
    }

    public object SecondPart()
    {
        char[,] map = GetMap();

        int bestScenicScore = 0;

        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                // Left
                int left = 0;
                for (int i = x - 1; i >= 0; i--)
                {
                    left++;
                    if (map[i, y] >= map[x, y])
                    {
                        break;
                    }
                }

                // Right
                int right = 0;
                for (int i = x + 1; i < map.GetLength(0); i++)
                {
                    right++;
                    if (map[i, y] >= map[x, y])
                    {
                        break;
                    }
                }

                // Top
                int top = 0;
                for (int i = y - 1; i >= 0; i--)
                {
                    top++;
                    if (map[x, i] >= map[x, y])
                    {
                        break;
                    }
                }

                // Bottom
                int bottom = 0;
                for (int i = y + 1; i < map.GetLength(1); i++)
                {
                    bottom++;
                    if (map[x, i] >= map[x, y])
                    {
                        break;
                    }
                }

                var scenicScore = left * right * top * bottom;
                if (scenicScore > bestScenicScore)
                {
                    bestScenicScore = scenicScore;
                }
            }
        }

        return bestScenicScore;
    }

    private char[,] GetMap()
    {
        char[,] map;
        var lines = InputReader.ReadAllLines(this);
        map = new char[lines[0].Length, lines.Length];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = lines[y][x];
            }
        }

        return map;
    }
}
