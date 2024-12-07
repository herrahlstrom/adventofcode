using System.Diagnostics;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 7, "Bridge Repair")]
internal class Year2024Day07 : IPuzzle
{
    [Answer(1298103531759L)]
    public object FirstPart()
    {
        return Solve((a, b) => [a + b, a * b]);
    }

    public object SecondPart()
    {
        return Solve((a, b) => [a + b, a * b, Concat(a, b)]);
    }

    private long Concat(long a, int b)
    {
        return b switch
        {
            < 10 => a * 10 + b,
            < 100 => a * 100 + b,
            < 1000 => a * 1000 + b,
            < 10000 => a * 10000 + b,
            _ => throw new UnreachableException()
        };
    }

    [Answer(140575048428831L)]
    private long Solve(Func<long, int, IEnumerable<long>> getValues)
    {
        List<int> numbers = [];

        long result = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            var span = line.AsSpan();
            int p = span.IndexOf(':');

            long target = long.Parse(span[..p]);
            numbers.Clear();

            var numbersSpan = span[(p + 1)..];
            foreach (Range r in numbersSpan.Split(' '))
            {
                var numberSpan = numbersSpan[r];
                if (numberSpan.Length > 0)
                    numbers.Add(int.Parse(numberSpan));
            }

            Queue<QueueItem> queue = [];
            queue.Enqueue(new QueueItem(numbers[0], 1));

            bool success = false;

            while (queue.TryDequeue(out var workItem))
            {
                var nn = getValues(workItem.Current, numbers[workItem.Next])
                    .Where(x => x <= target);

                var next = workItem.Next + 1;
                if (next == numbers.Count)
                {
                    if (nn.Any(x => x == target))
                    {
                        success = true;
                        break;
                    }
                }
                else if (next < numbers.Count)
                {
                    foreach (var n in nn)
                    {
                        queue.Enqueue(new QueueItem(n, next));
                    }
                }
                else
                {
                    throw new UnreachableException();
                }
            }

            if (success)
                result += target;
        }

        return result;
    }
}

internal record Equation(IList<int> Numbers);

internal record struct QueueItem(long Current, int Next);