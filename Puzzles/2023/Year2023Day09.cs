using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 9, "Mirage Maintenance")]
public class Year2023Day09 : IPuzzle
{
    [Answer(1974913025L)]
    public object FirstPart()
    {
        long sum = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            var numbers = line.Split(' ').Select(long.Parse).ToList();
            sum += ExtrapolateNext(numbers);
        }
        return sum;
    }

    public object SecondPart()
    {
        long sum = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            var numbers = line.Split(' ').Select(long.Parse).ToList();
            sum += ExtrapolatePrevious(numbers);
        }
        return sum;
    }

    private static List<long> CreateNextNumbers(List<long> numbers)
    {
        var next = new List<long>(numbers.Count - 1);
        for (int i = 1; i < numbers.Count; i++)
        {
            next.Add(numbers[i] - numbers[i - 1]);
        }

        return next;
    }

    private static long ExtrapolateNext(List<long> numbers)
    {
        if (numbers.All(x => x == 0))
        {
            return 0;
        }

        List<long> next = CreateNextNumbers(numbers);

        return numbers[^1] + ExtrapolateNext(next);
    }

    private static long ExtrapolatePrevious(List<long> numbers)
    {
        if (numbers.All(x => x == 0))
        {
            return 0;
        }

        List<long> next = CreateNextNumbers(numbers);

        return numbers[0] - ExtrapolatePrevious(next);
    }
}