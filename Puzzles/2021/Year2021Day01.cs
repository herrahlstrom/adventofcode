using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 1, "Sonar sweep")]
internal class Year2021Day01 : IPuzzle
{
    public object FirstPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        int result = 0;

        int lastValue = int.MaxValue;
        while (reader.ReadLine() is { } line)
        {
            int value = int.Parse(line);
            if (value > lastValue)
            {
                result++;
            }

            lastValue = value;
        }

        return result;
    }

    public object SecondPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        int[] sliding = { 0, 0, 0 };
        int result = 0;

        sliding[1] = int.Parse(reader.ReadLine() ?? throw new InvalidDataException());
        sliding[0] = int.Parse(reader.ReadLine() ?? throw new InvalidDataException());

        int lastSum = int.MaxValue;
        while (reader.ReadLine() is { } line)
        {
            int value = int.Parse(line);

            sliding[2] = sliding[1];
            sliding[1] = sliding[0];
            sliding[0] = value;

            int sum = sliding.Sum();

            if (sum > lastSum)
            {
                result++;
            }

            lastSum = sum;
        }

        return result;
    }
}