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

         List<Map> maps = [];

         lineEnumerator.MoveNext();
         while (lineEnumerator.MoveNext())
         {
            maps.Add(ReadMap(lineEnumerator));
         }


         throw new NotImplementedException();
      }

      public object SecondPart()
      {
         throw new NotImplementedException();
      }

      private static List<int> ReadSSV(ReadOnlySpan<char> span, int expectedCount = 0)
      {
         List<int> seeds = new(expectedCount);

         int a = 0;
         int b = 0;
         while (b < span.Length)
         {
            while (b < span.Length && span[b] != ' ')
            {
               b++;
            }

            seeds.Add(int.Parse(span[a..b]));
            a = b + 1;
            b = a;
         }

         return seeds;
      }

      private Map ReadMap(IEnumerator<string> lineEnumerator)
      {
         var rx = MapHeaderRx().Match(lineEnumerator.Current);

         var from = rx.Groups["from"].Value;
         var to = rx.Groups["to"].Value;

         var data = new List<MapData>();
         while(lineEnumerator.MoveNext() && lineEnumerator.Current.Length > 0)
         {
            var ssv = ReadSSV(lineEnumerator.Current.AsSpan(), 3);
            data.Add(new MapData(ssv[0], ssv[1], ssv[2]));
         }

         return new Map(from, to, data);
      }

      private static List<int> ReadSeeds(string line)
      {
         const string startText = "seeds: ";
         if (!line.StartsWith(startText)) { throw new InvalidOperationException(); }

         ReadOnlySpan<char> span = line.AsSpan();

         return ReadSSV(span[startText.Length..], 20);
      }

      private record struct Map(string From, string To, IReadOnlyCollection<MapData> MapData);
      private record struct MapData(int Source, int Destination, int Lenght);

      [GeneratedRegex("^(?<from>.+)-to-(?<to>.+) map:$")]
      private static partial Regex MapHeaderRx();
   }
}
