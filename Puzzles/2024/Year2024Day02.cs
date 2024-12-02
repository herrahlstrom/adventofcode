using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 2, "Red-Nosed Reports")]
public class Year2024Day02 : IPuzzle
{
    [Answer(564)]
    public object FirstPart()
    {
        return ReadItems().Count(numbers => IsSafe(numbers, numbers.Length));
    }

    [Answer(604)]
    public object SecondPart()
    {
        int[] buffer = new int[10];

        int safeCounter = 0;
        foreach (int[] numbers in ReadItems())
        {
            if (IsSafe(numbers, numbers.Length))
            {
                safeCounter++;
            }
            else
            {
                if (Enumerable.Range(0, numbers.Length).Any(i =>
                    {
                        Clone(numbers, buffer, i);
                        return IsSafe(buffer, numbers.Length - 1);
                    }))
                {
                    safeCounter++;
                }
            }
        }

        return safeCounter;
    }

    private void Clone(int[] source, int[] destination, int exceptPosition)
    {
        int j = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (i != exceptPosition)
            {
                destination[j++] = source[i];
            }
        }
    }

    private static bool IsSafe(int[] numbers, int length)
    {
        int sign = Math.Sign(numbers[1] - numbers[0]);
        return Enumerable.Range(1, length - 1).All(i => Math.Sign(numbers[i] - numbers[i - 1]) == sign &&
                                                        Math.Abs(numbers[i] - numbers[i - 1]) is >= 1 and <= 3);
    }

    private IEnumerable<int[]> ReadItems()
    {
        foreach (var line in InputReader.ReadLines(this))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var numbers = line.Split().Select(int.Parse).ToArray();
            yield return numbers;
        }
    }
}