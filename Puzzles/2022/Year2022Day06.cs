using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 6, "Tuning Trouble")]
internal class Year2022Day06 : IPuzzle
{
    public object FirstPart()
    {
        return FindStartOfPacketMarker(4);
    }

    public object SecondPart()
    {
        return FindStartOfPacketMarker(14);
    }

    private object FindStartOfPacketMarker(int size)
    {
        var map = new HashSet<char>(size);

        var input = InputReader.ReadAllText(this);
        for (int i = 0; i < input.Length; i++)
        {
            map.Clear();
            for (int j = 0; j < size; j++)
            {
                if (!map.Add(input[i + j]))
                {
                    break;
                }
            }

            if (map.Count == size)
            {
                return i + size;
            }
        }
        throw new NotSupportedException();
    }
}
