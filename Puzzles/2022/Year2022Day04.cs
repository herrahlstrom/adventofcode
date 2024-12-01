using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 4, "Camp Cleanup")]
internal class Year2022Day04 : IPuzzle
{
    public object FirstPart()
    {
        int result = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            var pair = GetRanges(line);

            if (pair.a.start <= pair.b.start && pair.a.end >= pair.b.end ||
                pair.b.start <= pair.a.start && pair.b.end >= pair.a.end)
            {
                result++;
            }

        }

        return result;
    }

    public object SecondPart()
    {
        int result = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            var pair = GetRanges(line);

            if (pair.a.start <= pair.b.end && pair.a.end >= pair.b.start ||
                pair.b.start <= pair.a.end && pair.b.end >= pair.a.start)
            {
                result++;
            }

        }

        return result;
    }

    private ((int start, int end) a, (int start, int end) b) GetRanges(string line)
    {
        var lineSpan = line.AsSpan();

        int leftDash = lineSpan.IndexOf('-');
        int pairSplitter = lineSpan.IndexOf(',');
        int rightDash = lineSpan.LastIndexOf('-');

        int aStart = int.Parse(lineSpan.Slice(0, leftDash));
        int aEnd = int.Parse(lineSpan.Slice(leftDash + 1, pairSplitter - leftDash - 1));
        int bStart = int.Parse(lineSpan.Slice(pairSplitter + 1, rightDash - pairSplitter - 1));
        int bEnd = int.Parse(lineSpan.Slice(rightDash + 1));

        return ((aStart, aEnd), (bStart, bEnd));
    }
}
