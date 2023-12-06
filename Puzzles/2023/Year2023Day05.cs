using AdventOfCode.Helper;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    [Puzzle(2023, 5, "If You Give A Seed A Fertilizer")]
    public partial class Year2023Day05 : IPuzzle
    {
        const string FirstStep = "seed";
        const string LastStep = "location";

        public object FirstPart()
        {
            IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

            //lineEnumerator.MoveNext();
            //List<SeedEntry> numbers = lineEnumerator.Current[7..].Split()
            //                                                     .Select(x => new SeedEntry(long.Parse(x)))
            //                                                     .ToList();

            //lineEnumerator.MoveNext();
            //Dictionary<string, Map> maps = ReadMaps(lineEnumerator).ToDictionary(x => x.From, x => x);

            //var buffer = new List<long>(numbers.Count);
            //string step = FirstStep;
            //while (step != LastStep)
            //{
            //    var map = maps[step];
            //    buffer.AddRange(numbers.Select(map.GetNumber));

            //    numbers.Clear();
            //    numbers.AddRange(buffer);
            //    buffer.Clear();

            //    step = map.To;
            //}

            //return numbers.Min();

            lineEnumerator.MoveNext();
            List<SeedEntry> seeds = lineEnumerator.Current[7..].Split()
                                                               .Select(x => new SeedEntry(long.Parse(x)))
                                                               .ToList();

            lineEnumerator.MoveNext();
            List<Map> maps = ReadMaps(lineEnumerator).ToList();

            return Calc(seeds, maps);
        }

        public object SecondPart()
        {
            throw new NotImplementedException();
            IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

            lineEnumerator.MoveNext();
            List<SeedEntry> seeds = ReadSeeds(lineEnumerator.Current).ToList();

            lineEnumerator.MoveNext();
            List<Map> maps = ReadMaps(lineEnumerator).ToList();
            
            return Calc(seeds, maps);

            IEnumerable<SeedEntry> ReadSeeds(string line)
            {
                var seedEnumerator = lineEnumerator.Current[7..].Split().Select(long.Parse).GetEnumerator();
                while (seedEnumerator.MoveNext())
                {
                    long value = seedEnumerator.Current;
                    seedEnumerator.MoveNext();
                    long count = seedEnumerator.Current;
                    yield return new SeedEntry(value, count);
                }
            }
        }

        private static object Calc(List<SeedEntry> seeds, List<Map> maps)
        {
            long max = maps[^1].MapData.Select(x => x.Destination + x.Lenght).Max();
            for (int i = 0; i < max; i++)
            {
                long next = i;

                for (int j = maps.Count - 1; j >= 0; j--)
                {
                    next = maps[j].GetRevNumber(next);
                }
                foreach (var seedEntry in seeds)
                {
                    if (seedEntry.Contains(next))
                    {
                        return i;
                    }
                }
            }

            throw new UnreachableException();
        }

        [GeneratedRegex("^(?<from>.+)-to-(?<to>.+) map:$")]
        private static partial Regex MapHeaderRx();

        private static List<long> ReadSeeds(string line)
        {
            const string startText = "seeds: ";
            if (!line.StartsWith(startText)) { throw new InvalidOperationException(); }

            ReadOnlySpan<char> span = line.AsSpan();

            return ReadSSV(span[startText.Length..], 20);
        }

        private static List<long> ReadSSV(ReadOnlySpan<char> span, int expectedCount = 0)
        {
            List<long> seeds = new(expectedCount);

            int a = 0;
            int b = 0;
            while (b < span.Length)
            {
                while (b < span.Length && span[b] != ' ')
                {
                    b++;
                }

                seeds.Add(long.Parse(span[a..b]));
                a = b + 1;
                b = a;
            }

            return seeds;
        }

        private Map ReadMap(IEnumerator<string> lineEnumerator)
        {
            var rx = MapHeaderRx().Match(lineEnumerator.Current);
            var map = new Map(rx.Groups["from"].Value, rx.Groups["to"].Value);

            while (lineEnumerator.MoveNext() && lineEnumerator.Current.Length > 0)
            {
                var ssv = ReadSSV(lineEnumerator.Current.AsSpan(), 3);
                map.MapData.Add(new MapData(ssv[1], ssv[0], ssv[2]));
            }

            return map;
        }

        private IEnumerable<Map> ReadMaps(IEnumerator<string> lineEnumerator)
        {
            while (lineEnumerator.MoveNext())
            {
                yield return ReadMap(lineEnumerator);
            }
        }

        private class SeedEntry(long source, long count=1)
        {
            public bool Contains(long value)
            {
                return value >= source && value < source + count;
            }
        }
        private class Map(string from, string to)
        {
            public string From { get; } = from;
            public IList<MapData> MapData { get; } = new List<MapData>();
            public string To { get; } = to;
            
            public long GetNumber(long number)
            {
                foreach (var data in MapData)
                {
                    if (number >= data.Source && number <= data.Source + data.Lenght)
                    {
                        return (data.Destination - data.Source);
                    }
                }
                return number;
            }

            public long GetRevNumber(long number)
            {
                foreach(var data in MapData)
                {
                    if(number >= data.Destination && number <= data.Destination + data.Lenght)
                    {
                        return number - (data.Destination - data.Source);
                    }
                }
                return number;
            }
        }

        private record struct MapData(long Source, long Destination, long Lenght);
    }
}
