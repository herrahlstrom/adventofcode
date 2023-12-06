using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles
{
   [Puzzle(2023, 5, "If You Give A Seed A Fertilizer")]
   public partial class Year2023Day05 : IPuzzle
   {
      const string FirstStep = "seed";
      const string LastStep = "location";

      [Answer(51580674L)]
      public object FirstPart()
      {
         IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

         lineEnumerator.MoveNext();
         List<long> seeds = lineEnumerator.Current[7..].Split()
                                                       .Select(long.Parse)
                                                       .ToList();

         lineEnumerator.MoveNext();
         List<Map> maps = ReadMaps(lineEnumerator).ToList();

         return SimpleSolution(seeds, maps);
      }

      public object SecondPart()
      {
         IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

         lineEnumerator.MoveNext();
         List<SeedEntry> seeds = new(10);
         var arr = lineEnumerator.Current[7..].Split();
         for (int i = 0; i < arr.Length; i += 2)
         {
            seeds.Add(new SeedEntry(long.Parse(arr[i]), int.Parse(arr[i + 1])));
         }

         lineEnumerator.MoveNext();
         List<Map> maps = ReadMaps(lineEnumerator).ToList();

         return ImprovedSolution(seeds, maps);
      }

      private static long ImprovedSolution(List<SeedEntry> seeds, List<Map> maps)
      {
         long max = maps[^1].MapData.Select(x => x.Destination + x.Lenght).Max();
         float nextPrint = 0.001f;
         bool hasResult = false;
         ConcurrentBag<long> result = [];

         const int blockSize = 100;

         for (int i = 0; i < max; i+= blockSize)
         {
            var p = (float)i/max;
            if(p>= nextPrint)
            {
               Console.WriteLine("{0:P1}", p);
               nextPrint += 0.001f;
            }

            Parallel.For(
               i,
               i + blockSize,
               blockStart =>
               {
                  long value = blockStart;

                  for(int j = maps.Count - 1; j >= 0; j--)
                  {
                     value = maps[j].GetRevNumber(value);
                  }
                  foreach(var seedEntry in seeds)
                  {
                     if(seedEntry.Contains(value))
                     {
                        hasResult = true;
                        result.Add(blockStart);
                     }
                  }
               });
            if (hasResult)
            {
               return result.Min(x => x);
            }
         }

         throw new UnreachableException();
      }

      [GeneratedRegex("^(?<from>.+)-to-(?<to>.+) map:$")]
      private static partial Regex MapHeaderRx();

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

      private static long SimpleSolution(List<long> seeds, List<Map> maps)
      {
         var numbers = new List<long>(seeds);
         var buffer = new List<long>(numbers.Count);

         foreach (var map in maps)
         {
            buffer.AddRange(numbers.Select(map.GetNumber));

            numbers.Clear();
            numbers.AddRange(buffer);
            buffer.Clear();
         }

         return numbers.Min();
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
                  return number + (data.Destination - data.Source);
               }
            }
            return number;
         }

         public long GetRevNumber(long number)
         {
            foreach (var data in MapData)
            {
               if (number >= data.Destination && number <= data.Destination + data.Lenght)
               {
                  return number - (data.Destination - data.Source);
               }
            }
            return number;
         }
      }

      private class SeedEntry(long source, int count = 1)
      {
         public int Count { get; } = count;
         public long Source { get; } = source;

         public bool Contains(long value)
         {
            return value >= Source && value < Source + Count;
         }
      }

      private record struct MapData(long Source, long Destination, long Lenght);
   }
}
