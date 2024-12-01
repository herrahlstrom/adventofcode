using System.Reflection;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 10, "Cathode-Ray Tube")]
internal class Year2022Day10 : IPuzzle
{
    public object FirstPart()
    {
        int cycle = 0;
        int x = 1;
        int nextCycleCheck = 20;

        int signalStrengthsSum = 0;

        foreach (var line in InputReader.ReadLines(this))
        {
            if (line == "noop")
            {
                IncCycles();
            }
            else
            {
                IncCycles();
                IncCycles();

                x += int.Parse(line[5..]);
            }
        }

        void IncCycles()
        {
            cycle++;
            if (cycle == nextCycleCheck)
            {
                signalStrengthsSum += cycle * x;
                nextCycleCheck += 40;
            }
        }

        return signalStrengthsSum;
    }

    public object SecondPart()
    {
        int cycle = 0;
        int x = 1;

        PuzzleAttribute puzzle = GetType().GetCustomAttribute<PuzzleAttribute>()!;
        Console.WriteLine("");
        Console.WriteLine("- Output from {0} {1}: {2} -", puzzle.Year, puzzle.Day, puzzle.Name);

        foreach (var line in InputReader.ReadLines(this))
        {
            if (line == "noop")
            {
                IncCycles();
            }
            else
            {
                IncCycles();
                IncCycles();

                x += int.Parse(line[5..]);
            }
        }
        
        Console.WriteLine("");

        void IncCycles()
        {
            cycle++;
            Console.Write(cycle == x || cycle == x + 1 || cycle == x + 2 ? "#" : ".");

            if (cycle == 40)
            {
                Console.WriteLine();
                cycle = 0;
            }
        }

        // Catched from console output
        return "FCJAPJRE";
    }
}
