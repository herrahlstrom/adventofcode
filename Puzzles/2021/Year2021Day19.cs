using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 19, "Beacon Scanner")]
internal class Year2021Day19 : IPuzzle
{
    private List<Scanner> scanners = new();

    public object FirstPart()
    {
        ReadInput();

        throw new NotImplementedException();
    }

    public void ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);

        var scannerLineRz = new Regex(@"--- scanner (\d+) ---$");

        Scanner? current = null;
        while (!reader.EndOfStream && reader.ReadLine() is { } line)
        {
            if (line == "")
            {
                current = null;
            }
            else if (current is null)
            {
                Match m;
                if ((m = scannerLineRz.Match(line)).Success == false)
                {
                    throw new InvalidDataException("Invalid scanner head: " + line);
                }

                current = new Scanner()
                {
                    Id = int.Parse(m.Groups[1].Value)
                };
                scanners.Add(current);
            }
            else
            {
                int[] values = line.Split(',').Select(int.Parse).ToArray();
                current.Beacons.Add(new Beacon() { X = values[0], Y = values[1], Z = values[2] });
            }
        }
    }

    public object SecondPart() => throw new NotImplementedException();

    private class Scanner
    {
        public int Id { get; init; }

        public List<Beacon> Beacons { get; } = new();
    }

    private record Beacon
    {
        public int X { get; init; }

        public int Y { get; init; }

        public int Z { get; init; }

        public double GetDistance(Beacon other)
        {
            return Math.Sqrt(Math.Pow(other.X - X, 2) +
                             Math.Pow(other.Y - Y, 2) +
                             Math.Pow(other.Z - Z, 2));
        }
    }
}
