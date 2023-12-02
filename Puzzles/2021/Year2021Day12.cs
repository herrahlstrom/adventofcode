using System.Diagnostics;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 12, "Passage Pathing")]
internal class Year2021Day12 : IPuzzle
{
    public object FirstPart() => GetPaths(GetStartCaves(), 0);

    public object SecondPart() => GetPaths(GetStartCaves(), 1);

    private int GetPaths(Cave start, int smallCaveRevisitCounter)
    {
        var wave = new List<CavePath>
        {
            new(start)
            {
                SmallCaveRevisitCounter = smallCaveRevisitCounter
            }
        };

        List<CavePath> buffer = new(1024);
        int count = 0;

        do
        {
            buffer.Clear();

            foreach (CavePath p in wave)
            {
                foreach (Cave c in p.Head.Connections)
                {
                    if (c.Name == "end")
                    {
                        count++;
                    }
                    else if (p.CanEnter(c))
                    {
                        buffer.Add(p.Continue(c));
                    }
                }
            }

            wave.Clear();
            wave.AddRange(buffer);
        } while (wave.Count > 0);

        return count;
    }

    private Cave GetStartCaves()
    {
        using var reader = InputReader.OpenStreamReader(this);

        var caves = new Dictionary<string, Cave>();
        Cave? startCave = null;

        while (!reader.EndOfStream && reader.ReadLine() is { } line)
        {
            string[] arr = line.Split('-');

            if (caves.TryGetValue(arr[0], out Cave? a) == false)
            {
                a = new Cave(arr[0]);
                caves.Add(a.Name, a);

                if (a.Name == "start")
                {
                    startCave = a;
                }
            }

            if (caves.TryGetValue(arr[1], out Cave? b) == false)
            {
                b = new Cave(arr[1]);
                caves.Add(b.Name, b);

                if (b.Name == "start")
                {
                    startCave = b;
                }
            }

            a.AddConnection(b);
            b.AddConnection(a);
        }

        return startCave ?? throw new UnreachableException();
    }

    [DebuggerDisplay("{Name}")]
    private class Cave
    {
        private readonly List<Cave> _connections = new();

        public Cave(string name)
        {
            Name = name;
            IsBig = Name.All(char.IsUpper);
        }

        public IEnumerable<Cave> Connections => _connections;

        public bool IsBig { get; }

        public string Name { get; }

        public void AddConnection(Cave other)
        {
            if (Connections.Any(x => x.Name == other.Name))
            {
                return;
            }

            _connections.Add(other);
        }

        public override bool Equals(object? obj) => obj is Cave other && Equals(other);

        public override int GetHashCode() => Name.GetHashCode();

        private bool Equals(Cave other) => Name == other.Name;
    }

    private class CavePath
    {
        private readonly HashSet<string> _tailHash;

        public CavePath(Cave start)
        {
            Head = start;
            _tailHash = new HashSet<string> { start.Name };
        }

        private CavePath(CavePath source, Cave head)
        {
            Head = head;

            if (head.IsBig)
            {
                SmallCaveRevisitCounter = source.SmallCaveRevisitCounter;
            }
            else if (source._tailHash.Contains(head.Name))
            {
                SmallCaveRevisitCounter = source.SmallCaveRevisitCounter - 1;
            }
            else
            {
                SmallCaveRevisitCounter = source.SmallCaveRevisitCounter;
            }

            _tailHash = new HashSet<string>(source._tailHash.Append(head.Name));
        }

        public Cave Head { get; }
        public int SmallCaveRevisitCounter { get; init; }

        public bool CanEnter(Cave cave)
        {
            if (cave.Name == "start")
            {
                return false;
            }

            if (cave.IsBig)
            {
                return true;
            }

            if (SmallCaveRevisitCounter > 0)
            {
                return true;
            }

            return _tailHash.Contains(cave.Name) == false;
        }

        public CavePath Continue(Cave cave)
        {
            return new CavePath(this, cave);
        }
    }
}