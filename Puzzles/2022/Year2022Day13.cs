using AdventOfCode.Helper;
using System;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 13, "Distress Signal")]
internal class Year2022Day13 : IPuzzle
{
    public object FirstPart()
    {
        var packages = ReadPackages();

        int sum = 0;
        for(int i = 0, index = 1; i < packages.Count; i += 2, index++)
        {
            var a = packages[i];
            var b = packages[i + 1];
            if(a.CompareTo(b) <= 0)
            {
                sum += index;
            }
        }

        return sum;
    }

    public object SecondPart()
    {
        IPackageData divider1 = Parse("[[2]]");
        IPackageData divider2 = Parse("[[6]]");

        var packages = ReadPackages()
                       .Append(divider1)
                       .Append(divider2)
                       .ToList();

        packages.Sort();

        int decoderKey = 1;
        for(int i = 0; i < packages.Count; i++)
        {
            if(packages[i] == divider1 || packages[i] == divider2)
            {
                decoderKey *= (i + 1);
            }
        }

        return decoderKey;
    }

    IPackageData Parse(string line)
    {
        if(line.StartsWith('[') && line.EndsWith(']'))
        {
            List<IPackageData> items = new();

            StringBuilder str = new(4);
            int level = 0;
            for(int i = 1; i < line.Length - 1; i++)
            {
                if(line[i] == ',' && level == 0)
                {
                    items.Add(Parse(str.ToString()));
                    str.Clear();
                    continue;
                }

                if(line[i] == '[')
                {
                    level++;
                }
                if(line[i] == ']')
                {
                    level--;
                }
                str.Append(line[i]);
            }
            if(str.Length > 0)
            {
                items.Add(Parse(str.ToString()));
            }

            return new PackageGroup() { Values = items.ToArray() };
        }
        else if(int.TryParse(line, out int number))
        {
            return new PackageValue() { Value = number };
        }

        throw new NotImplementedException();
    }

    private List<IPackageData> ReadPackages()
    {
        StreamReader reader = InputReader.OpenStreamReader(this);

        List<IPackageData> packages = new();
        while(!reader.EndOfStream)
        {
            if(reader.ReadLine() is { Length: > 0 } line)
            {
                packages.Add(Parse(line));
            }
        }

        return packages;
    }

    interface IPackageData : IComparable<IPackageData>
    {
    }

    class PackageGroup : IPackageData, IComparable<PackageGroup>, IComparable<PackageValue>
    {
        public IPackageData[] Values { get; set; } = Array.Empty<IPackageData>();

        public int CompareTo(PackageGroup? other)
        {
            var e1 = Values.GetEnumerator();
            var e2 = other!.Values.GetEnumerator();

            while(true)
            {
                if(e1.MoveNext())
                {
                    if(e2.MoveNext())
                    {
                        var a = (IPackageData)e1.Current;
                        var b = (IPackageData)e2.Current;
                        var cmp = a.CompareTo(b);
                        if(cmp != 0)
                        {
                            return cmp;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if(e2.MoveNext())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public int CompareTo(PackageValue? other)
        {
            return CompareTo(new PackageGroup { Values = new[] { other! } });
        }

        public int CompareTo(IPackageData? other)
        {
            return other switch
            {
                PackageValue v => CompareTo(v),
                PackageGroup g => CompareTo(g),
                _ => throw new UnreachableException()
            };
        }

        public override string ToString()
        {
            return $"[{string.Join(",", Values.Select(x => x.ToString()))}]";
        }
    }

    [DebuggerDisplay("{Value}")]
    class PackageValue : IPackageData, IComparable<PackageValue>, IComparable<PackageGroup>
    {
        public int Value { get; set; }

        public int CompareTo(PackageValue? other)
        {
            return Value.CompareTo(other!.Value);
        }

        public int CompareTo(PackageGroup? other)
        {
            return new PackageGroup { Values = new[] { this } }.CompareTo(other);
        }

        public int CompareTo(IPackageData? other)
        {
            return other switch
            {
                PackageValue v => CompareTo(v),
                PackageGroup g => CompareTo(g),
                _ => throw new UnreachableException()
            };
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
