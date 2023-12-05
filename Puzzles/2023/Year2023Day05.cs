using AdventOfCode.Helper;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    [Puzzle(2023, 5, "If You Give A Seed A Fertilizer")]
    public partial class Year2023Day05 : IPuzzle
    {
        public object FirstPart()
        {
            IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

            lineEnumerator.MoveNext();
            var seeds = ReadSeeds(lineEnumerator.Current);

            lineEnumerator.MoveNext();
            Dictionary<string, Map> maps = ReadMaps(lineEnumerator).ToDictionary(x => x.From, x => x);

            return Calc(seeds, maps);
        }

        public object SecondPart()
        {
            IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

            lineEnumerator.MoveNext();
            var seeds = ReadSeeds(lineEnumerator.Current);

            lineEnumerator.MoveNext();
            Dictionary<string, Map> maps = ReadMaps(lineEnumerator).ToDictionary(x => x.From, x => x);

            return Calc(GetSeeds(seeds), maps);

            IEnumerable<long> GetSeeds(List<long> seedsInput)
            {
                var enumerator = seedsInput.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    long n = enumerator.Current;
                    enumerator.MoveNext();
                    var count = enumerator.Current;

                    while (count > 0)
                    {
                        yield return n;
                        n++;
                        count--;
                    }
                }
            }
        }

        private static object Calc(IEnumerable<long> input, Dictionary<string, Map> maps)
        {
            List<long> numbers = input.ToList();

            Console.WriteLine("Start process {0} seeds", numbers.Count);

            var buffer = new List<long>(numbers.Count);
            string step = "seed";
            while (step != "location")
            {
                Console.WriteLine(step);

                var map = maps[step];
                buffer.AddRange(numbers.Select(map.GetNumber));

                numbers.Clear();
                numbers.AddRange(buffer);
                buffer.Clear();

                step = map.To;
            }

            return numbers.Min();
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
                        number += (data.Destination - data.Source);
                        return number;
                    }
                }
                return number;
            }
        }

        private record struct MapData(long Source, long Destination, long Lenght);
    }
}
