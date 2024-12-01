using System.Diagnostics;
using System.Text;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 16, "Packet Decoder")]
internal class Year2021Day16 : IPuzzle
{
    private string _bits = "";

    public object FirstPart()
    {
        ReadInput();

        return ReadPackage(_bits, out _).GetVersionSum();
    }

    public void ReadInput()
    {
        string hex = InputReader.ReadAllText(this);

        StringBuilder bits = new(hex.Length * 4);
        foreach (char c in hex)
        {
            if(c == '\n' || c == ' ')
            {
                continue;
            }

            bits.Append(c switch
            {
                '0' => "0000",
                '1' => "0001",
                '2' => "0010",
                '3' => "0011",
                '4' => "0100",
                '5' => "0101",
                '6' => "0110",
                '7' => "0111",
                '8' => "1000",
                '9' => "1001",
                'A' => "1010",
                'B' => "1011",
                'C' => "1100",
                'D' => "1101",
                'E' => "1110",
                'F' => "1111",
                _ => throw new NotSupportedException()
            });
        }

        _bits = bits.ToString();
    }

    public object SecondPart() => ReadPackage(_bits.AsSpan(), out _).GetValue();

    private static LiteralValue ReadLiteralValue(int version, ReadOnlySpan<char> data, out int size)
    {
        StringBuilder resultBits = new(data.Length);
        size = 0;

        for (int i = 0; ; i += 5)
        {
            size += 5;

            resultBits.Append(data[i + 1]);
            resultBits.Append(data[i + 2]);
            resultBits.Append(data[i + 3]);
            resultBits.Append(data[i + 4]);

            if (data[i] == '1')
            {
                continue;
            }

            long value = Convert.ToInt64(resultBits.ToString(), 2);

            return new LiteralValue
            {
                Version = version,
                Value = value
            };
        }
    }

    private OperatorPackage ReadOperatorPackage(int version, int typeId, ReadOnlySpan<char> bits, out int size)
    {
        char lengthTypeId = bits[0];
        size = 1;

        OperatorPackage result = typeId switch
        {
            0 => new SumOperatorPackage { Version = version },
            1 => new ProductOperatorPackage { Version = version },
            2 => new MinOperatorPackage { Version = version },
            3 => new MaxOperatorPackage { Version = version },
            5 => new GreaterThanOperatorPackage { Version = version },
            6 => new LessThanOperatorPackage { Version = version },
            7 => new EqualOperatorPackage { Version = version },
            _ => throw new NotSupportedException()
        };


        ReadOnlySpan<char> content;
        int maxPackages;

        if (lengthTypeId == '0')
        {
            int contentSize = Convert.ToInt32(bits.Slice(1, 15).ToString(), 2);
            content = bits.Slice(16, contentSize);
            maxPackages = int.MaxValue;

            size += 15;
        }
        else if (lengthTypeId == '1')
        {
            maxPackages = Convert.ToInt32(bits.Slice(1, 11).ToString(), 2);
            content = bits.Slice(12);

            size += 11;
        }
        else
        {
            throw new NotSupportedException();
        }

        while (content.Length > 0 && maxPackages > 0)
        {
            Package package = ReadPackage(content, out int packageSize);

            content = content[packageSize..];
            maxPackages--;

            result.Packages.Add(package);

            size += packageSize;
        }

        return result;
    }

    private Package ReadPackage(ReadOnlySpan<char> data, out int size)
    {
        int version = Convert.ToInt32(data[..3].ToString(), 2);
        int typeId = Convert.ToInt32(data[3..6].ToString(), 2);
        int packageSize;

        Package package = typeId switch
        {
            4 => ReadLiteralValue(version, data[6..], out packageSize),
            _ => ReadOperatorPackage(version, typeId, data[6..], out packageSize)
        };

        size = packageSize + 6;

        return package;
    }

    private class SumOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages.Sum(x => x.GetValue());
    }

    private class ProductOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages.Aggregate(1L, (n, package) => n * package.GetValue());
    }

    private class MinOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages.Min(x => x.GetValue());
    }

    private class MaxOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages.Max(x => x.GetValue());
    }

    private class GreaterThanOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages[0].GetValue() > Packages[1].GetValue() ? 1 : 0;
    }

    private class LessThanOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages[0].GetValue() < Packages[1].GetValue() ? 1 : 0;
    }

    private class EqualOperatorPackage : OperatorPackage
    {
        public override long GetValue() => Packages[0].GetValue() == Packages[1].GetValue() ? 1 : 0;
    }

    private abstract class OperatorPackage : Package
    {
        public List<Package> Packages { get; } = new();
        public override long GetVersionSum() => Version + Packages.Sum(x => x.GetVersionSum());
    }

    private abstract class Package
    {
        public int Version { get; init; }
        public abstract long GetValue();
        public abstract long GetVersionSum();
    }

    [DebuggerDisplay("{Value}")]
    private class LiteralValue : Package
    {
        public long Value { get; init; }
        public override long GetValue() => Value;
        public override long GetVersionSum() => Version;
    }
}