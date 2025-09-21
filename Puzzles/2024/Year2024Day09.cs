using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 9, "Disk Fragmenter")]
internal class Year2024Day09 : IPuzzle
{
    private readonly string _input;

    public Year2024Day09()
    {
        _input = InputReader.ReadAllText(this);
    }

    [Answer(6200294120911L)]
    public object FirstPart()
    {
        var enumerator = new FirstPartEnumerator(_input);

        long result = 0;
        var position = 0;
        while (enumerator.TryReadNext(out var fileId))
        {
            result += position * fileId;
            position++;
        }

        Console.WriteLine();
        return result;
    }

    [Answer(6227018762750L)]
    public object SecondPart()
    {
        List<Block> disk = [.. ReadDataBlock(_input)];

        for (var fromEnd = disk.Count - 1; fromEnd > 0; fromEnd--)
        {
            if (disk[fromEnd].FileId is null)
                continue;

            for (var fromStart = 0; fromStart < fromEnd; fromStart++)
            {
                if (disk[fromStart].FileId is not null)
                    continue;
                if (disk[fromStart].Count < disk[fromEnd].Count)
                    continue;

                var file = disk[fromEnd];
                if (disk[fromEnd - 1].FileId is null)
                {
                    disk[fromEnd - 1].Count += file.Count;
                    disk.RemoveAt(fromEnd);
                }
                else if (disk[fromEnd + 1].FileId is null)
                {
                    disk[fromEnd + 1].Count += file.Count;
                    disk.RemoveAt(fromEnd);
                }
                else
                {
                    disk[fromEnd] = new Block(null) { Count = file.Count };
                }

                if (disk[fromStart].Count == file.Count)
                {
                    disk[fromStart] = file;
                }
                else
                {
                    disk[fromStart].Count -= file.Count;
                    disk.Insert(fromStart, file);
                }

                break;
            }
        }

        long result = 0;
        var position = 0;
        foreach (var block in disk)
        {
            for (var i = 0; i < block.Count; i++)
            {
                if (block.FileId.HasValue)
                    result += position * block.FileId.Value;
                position++;
            }
        }

        return result;
    }

    private static IEnumerable<Block> ReadDataBlock(string data)
    {
        var fileId = 0;
        for (var i = 0; i < data.Length; i++)
        {
            yield return i % 2 == 0
                ? new Block(fileId++) { Count = data[i] - 48 }
                : new Block(null) { Count = data[i] - 48 };
        }
    }

    private class FirstPartEnumerator
    {
        private readonly List<Block> _disk;
        private int _fromEnd;
        private int _fromStart;

        public FirstPartEnumerator(string data)
        {
            _disk = [.. ReadDataBlock(data)];

            _fromStart = 0;
            _fromEnd = _disk.Count - 1;
        }

        public bool TryReadNext(out int value)
        {
            // Move until next non-completed block
            while (_disk[_fromStart].Count < 1 && _fromStart <= _fromEnd)
            {
                _fromStart++;
            }

            // Are we done?
            if (_fromStart > _fromEnd)
            {
                value = 0;
                return false;
            }

            // Consume one from current block
            var currentFromStart = _disk[_fromStart];
            currentFromStart.Count -= 1;

            if (currentFromStart.FileId.HasValue)
            {
                // File
                value = currentFromStart.FileId.Value;
                return true;
            }

            // Free space, read file from end
            while (_fromEnd > _fromStart)
            {
                // Skip until non-complete file is found
                if (_disk[_fromEnd] is { Count: > 0, FileId: not null } currentFromEnd)
                {
                    currentFromEnd.Count -= 1;

                    value = currentFromEnd.FileId.Value;
                    return true;
                }

                _fromEnd--;
            }

            value = 0;
            return false;
        }
    }

    private record Block(int? FileId)
    {
        public int Count { get; set; }
    }
}