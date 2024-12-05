using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 5, "Print Queue")]
internal class Year2024Day05 : IPuzzle
{
    [Answer(6612)]
    public object FirstPart()
    {
        using var lines = InputReader.ReadLines(this).GetEnumerator();

        HashSet<Rule> pageOrderingRules = ReadPageOrderingRules(lines).ToHashSet();

        int sum = 0;

        while (lines.MoveNext())
        {
            Span<int> pageNumbers = lines.Current.Split(',').Select(int.Parse).ToArray().AsSpan();

            if (!IsCorrectly(pageNumbers, pageOrderingRules))
                continue;

            sum += pageNumbers[(pageNumbers.Length - 1) / 2];
        }

        return sum;
    }

    [Answer(4944)]
    public object SecondPart()
    {
        using var lines = InputReader.ReadLines(this).GetEnumerator();

        HashSet<Rule> pageOrderingRules = ReadPageOrderingRules(lines).ToHashSet();

        int sum = 0;

        while (lines.MoveNext())
        {
            int[] array = lines.Current.Split(',').Select(int.Parse).ToArray();
            Span<int> pageNumbers = array.AsSpan();

            if (IsCorrectly(pageNumbers, pageOrderingRules))
                continue;

            List<int> correct = new List<int>(array.Length);
            while (correct.Count < array.Length)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var isNext = array
                        .Where(x => x >= 0)
                        .Where(x => x != array[i])
                        .All(x => pageOrderingRules.Contains(new Rule(array[i], x)));
                    if (isNext)
                    {
                        correct.Add(array[i]);
                        array[i] = -1;
                        break;
                    }
                }
            }

            sum += correct[(correct.Count - 1) / 2];
        }

        return sum;
    }
    
    private bool IsCorrectly(ReadOnlySpan<int> pageNumbers, ICollection<Rule> pageOrderingRules)
    {
        bool correctly = true;

        for (int i = 0; i < pageNumbers.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (pageOrderingRules.Contains(new Rule(pageNumbers[j], pageNumbers[i])))
                    continue;
                correctly = false;
                break;
            }

            if (!correctly)
                break;

            for (int j = i + 1; j < pageNumbers.Length; j++)
            {
                if (pageOrderingRules.Contains(new Rule(pageNumbers[i], pageNumbers[j])))
                    continue;
                correctly = false;
                break;
            }

            if (!correctly)
                break;
        }

        return correctly;
    }

    private static List<Rule> ReadPageOrderingRules(IEnumerator<string> lines)
    {
        List<Rule> pageOrderingRules = [];

        while (lines.MoveNext() && lines.Current != "")
        {
            ReadOnlySpan<char> lineSpan = lines.Current.AsSpan();
            int p = lineSpan.IndexOf('|');
            int x = int.Parse(lineSpan[..p]);
            int y = int.Parse(lineSpan[(p + 1)..]);
            pageOrderingRules.Add(new Rule(x, y));
        }

        return pageOrderingRules;
    }

    private record struct Rule(int A, int B);
}