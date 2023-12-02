using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 6, "Lanternfish")]
internal class Year2021Day06 : IPuzzle
{
    private int[] _initValues = Array.Empty<int>();

    public object FirstPart()
    {
        ReadInput();
        return SimulateDays(80);
    }

    public void ReadInput()
    {
        _initValues = InputReader.ReadAllText(this)
            .Split(',').Select(int.Parse).ToArray();
    }

    public object SecondPart() => SimulateDays(256);

    private long SimulateDays(int days)
    {
        long[] values = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        foreach (int n in _initValues)
        {
            values[n]++;
        }

        for (int d = 1; d <= days; d++)
        {
            long born = values[0];
            values[0] = values[1];
            values[1] = values[2];
            values[2] = values[3];
            values[3] = values[4];
            values[4] = values[5];
            values[5] = values[6];
            values[6] = values[7] + born;
            values[7] = values[8];
            values[8] = born;
        }

        return values.Sum();
    }
}