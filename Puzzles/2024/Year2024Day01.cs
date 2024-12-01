using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 1, "Historian Hysteria")]
public class Year2024Day01 : IPuzzle
{
    [Answer(1603498L)]
    public object FirstPart()
    {
        List<int> aList = [];
        List<int> bList = [];
        foreach ((int a, int b) in ReadItems())
        {
            aList.Add(a);
            bList.Add(b);
        }

        aList.Sort();
        bList.Sort();

        long diff = 0;
        for (int i = 0; i < aList.Count; i++)
        {
            var a = aList[i];
            var b = bList[i];
            diff += a > b ? a - b : b - a;
        }

        return diff;
    }

    [Answer(25574739L)]
    public object SecondPart()
    {
        List<int> aList = [];
        Dictionary<int, int> bDictionary = [];
        foreach ((int a, int b) in ReadItems())
        {
            aList.Add(a);
            if (!bDictionary.TryAdd(b, 1))
                bDictionary[b]++;
        }

        long score = 0;
        foreach (int a in aList)
        {
            if (bDictionary.TryGetValue(a, out int count))
                score += a * count;
        }

        return score;
    }

    private IEnumerable<(int, int)> ReadItems()
    {
        foreach (var line in InputReader.ReadLines(this))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var span = line.AsSpan();

            int p = span.IndexOf(' ');
            int a = int.Parse(span[..p]);
            int b = int.Parse(span[p..]);

            yield return (a, b);
        }
    }
}