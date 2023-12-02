using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 2, "Dive")]
internal class Year2021Day02 : IPuzzle
{
    public object FirstPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        Point pos = new(0, 0);

        while (reader.ReadLine() is { } line)
        {
            string[] arr = line.Split();
            string command = arr[0];
            int value = int.Parse(arr[1]);

            pos += command switch
            {
                "forward" => new Point(value, 0),
                "down" => new Point(0, value),
                "up" => new Point(0, -value),
                _ => throw new NotImplementedException()
            };
        }

        return pos.X * pos.Y;
    }

    public object SecondPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        Point pos = new(0, 0);
        int aim = 0;

        while (reader.ReadLine() is { } line)
        {
            string[] arr = line.Split();
            string command = arr[0];
            int value = int.Parse(arr[1]);

            switch (command)
            {
                case "forward":
                    pos += new Point(value, value * aim);
                    break;
                case "down":
                    aim += value;
                    break;
                case "up":
                    aim -= value;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return pos.X * pos.Y;
    }
}