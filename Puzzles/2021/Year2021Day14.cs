using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 14, "Extended Polymerization")]
internal class Year2021Day14 : IPuzzle
{
    private readonly List<string> _insertions = new();
    private string _polymerTemplate = "";

    public object FirstPart()
    {
        ReadInput();

        return GetResult(10);
    }

    public void ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);

        _polymerTemplate = reader.ReadLine() ?? throw new InvalidDataException();
        reader.ReadLine();

        while (!reader.EndOfStream && reader.ReadLine() is { Length: > 0 } line)
        {
            _insertions.Add(line[..2] + line[6..]);
        }
    }

    public object SecondPart()
    {
        return GetResult(40);
    }

    private long GetResult(int steps)
    {
        var mapping = _insertions.ToDictionary(x => x[..2], x => new
        {
            a = new string(new[] { x[0], x[2] }),
            b = new string(new[] { x[2], x[1] }),
            inserted = x[2]
        });
        Dictionary<string, long> pairCount = new(_insertions.Count);
        IDictionary<char, long> charCount =
            _polymerTemplate.GroupBy(c => c).ToDictionary(x => x.Key, x => x.LongCount());

        for (int i = 0; i < _polymerTemplate.Length - 1; i++)
        {
            string a = _polymerTemplate.Substring(i, 2);
            pairCount[a] = pairCount.TryGetValue(a, out long c) ? c + 1 : 1;
        }

        for (int step = 1; step <= steps; step++)
        {
            var nexts = pairCount.Where(x => x.Value > 0).ToList();

            foreach ((string key, long count) in nexts)
            {
                var pairMapping = mapping[key];

                pairCount[key] -= count;

                if (!pairCount.TryGetValue(pairMapping.a, out long aCount))
                {
                    aCount = 0;
                }

                pairCount[pairMapping.a] = aCount + count;

                if (!pairCount.TryGetValue(pairMapping.b, out long bCount))
                {
                    bCount = 0;
                }

                pairCount[pairMapping.b] = bCount + count;

                if (!charCount.TryGetValue(pairMapping.inserted, out long charMappedCount))
                {
                    charMappedCount = 0;
                }

                charCount[pairMapping.inserted] = charMappedCount + count;
            }
        }

        var ordered = charCount.OrderByDescending(x => x.Value).ToList();
        long result = ordered.First().Value - ordered.Last().Value;

        return result;
    }
}