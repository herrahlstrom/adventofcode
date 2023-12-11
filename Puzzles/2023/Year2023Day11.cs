using System;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 11, "Cosmic Expansion")]
internal class Year2023Day11 : IPuzzle
{
    [Answer(9623138L)]
    public object FirstPart()
    {
        return Calculate(2);
    }

    [Answer(726820169514)]
    public object SecondPart()
    {
        return Calculate(1000000);
    }

    private long Calculate(long expandedSize)
    {
        var map = InputReader.ReadMap(this);

        bool[] expandedHorizontals = Enumerable.Repeat(true, map.GetLength(0)).ToArray();
        bool[] expandedVerticals = Enumerable.Repeat(true, map.GetLength(1)).ToArray();
        List<Point> galaxies = [];

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] == '#')
                {
                    galaxies.Add(new Point(x, y));

                    expandedVerticals[x] = false;
                    expandedHorizontals[y] = false;
                }
            }
        }

        long totalCosts = 0;

        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            var galaxy = galaxies[i];
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var otherGalaxy = galaxies[j];

                var minX = Math.Min(galaxy.X, otherGalaxy.X);
                var minY = Math.Min(galaxy.Y, otherGalaxy.Y);
                var maxX = minX + Math.Abs(otherGalaxy.X - galaxy.X);
                var maxY = minY + Math.Abs(otherGalaxy.Y - galaxy.Y);

                for (int x = minX; x < maxX; x++)
                {
                    totalCosts += expandedVerticals[x] ? expandedSize : 1;
                }
                for (int y = minY; y < maxY; y++)
                {
                    totalCosts += expandedHorizontals[y] ? expandedSize : 1;
                }
            }
        }

        return totalCosts;
    }
}
