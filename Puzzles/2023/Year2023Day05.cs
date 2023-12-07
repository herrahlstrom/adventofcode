using System;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles
{
   [Puzzle(2023, 5, "If You Give A Seed A Fertilizer")]
   public partial class Year2023Day05 : IPuzzle
   {
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

      [Answer(99751240L)]
      public object SecondPart()
      {
         IEnumerator<string> lineEnumerator = InputReader.ReadLines(this).GetEnumerator();

         lineEnumerator.MoveNext();
         List<ValueRange> seeds = new(10);
         var arr = lineEnumerator.Current[7..].Split();
         for (int i = 0; i < arr.Length; i += 2)
         {
            seeds.Add(new ValueRange(long.Parse(arr[i]), long.Parse(arr[i]) + int.Parse(arr[i + 1])));
         }

         lineEnumerator.MoveNext();
         List<Map> maps = ReadMaps(lineEnumerator).ToList();

         return SplitRangeSolution(seeds, maps);
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
            buffer.AddRange(numbers.Select(number => Transform(number)));

            numbers.Clear();
            numbers.AddRange(buffer);
            buffer.Clear();

            long Transform(long number)
            {
               return map.Transformers
                         .Where(data => number >= data.Range.Start && number <= data.Range.End)
                         .Select(data => data.Destination - data.Range.Start)
                         .FirstOrDefault() + number;
            }
         }

         return numbers.Min();
      }

      private static long SplitRangeSolution(List<ValueRange> seeds, List<Map> maps)
      {
         var a = new List<ValueRange>(seeds.Count);
         var b = new List<ValueRange>(seeds.Count);
         a.AddRange(seeds);

         foreach (var map in maps)
         {
            foreach (var val in a)
            {
               List<ValueRange> workingValues = [val];
               List<ValueRange> buffer = [];

               foreach (var transformer in map.Transformers)
               {
                  foreach (var workVal in workingValues)
                  {
                     if (workVal.End < transformer.Range.Start || workVal.Start > transformer.Range.End)
                     {
                        buffer.Add(workVal);
                        continue;
                     }

                     if (workVal.Start < transformer.Range.Start)
                     {
                        buffer.Add(new ValueRange(workVal.Start, transformer.Range.Start - 1));
                     }

                     long crossStart = Math.Max(workVal.Start, transformer.Range.Start);
                     long crossEnd = Math.Min(workVal.End, transformer.Range.End);
                     var delta = transformer.Destination - transformer.Range.Start;
                     b.Add(new ValueRange(crossStart + delta, crossEnd + delta));

                     if (workVal.End > transformer.Range.End)
                     {
                        buffer.Add(new ValueRange(transformer.Range.End + 1, workVal.End));
                     }
                  }

                  workingValues.Clear();
                  workingValues.AddRange(buffer);
                  buffer.Clear();

                  if (workingValues.Count == 0)
                  {
                     break;
                  }
               }

               // Add the non transformed values
               b.AddRange(workingValues);
            }

            a.Clear();
            a.AddRange(b);
            b.Clear();
         }

         return a.Min(x => x.Start);
      }

      private Map ReadMap(IEnumerator<string> lineEnumerator)
      {
         var rx = MapHeaderRx().Match(lineEnumerator.Current);
         var map = new Map(rx.Groups["from"].Value, rx.Groups["to"].Value);

         while (lineEnumerator.MoveNext() && lineEnumerator.Current.Length > 0)
         {
            var ssv = ReadSSV(lineEnumerator.Current.AsSpan(), 3);
            map.Transformers.Add(new Transformer(new ValueRange(ssv[1], ssv[1] + ssv[2] - 1), ssv[0]));
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
         public string To { get; } = to;
         public IList<Transformer> Transformers { get; } = new List<Transformer>();
      }

      private record struct ValueRange(long Start, long End);

      private record struct Transformer(ValueRange Range, long Destination);
   }
}
