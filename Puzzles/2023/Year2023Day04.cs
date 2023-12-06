using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles
{
   [Puzzle(2023, 4, "Scratchcards")]
   public class Year2023Day04 : IPuzzle
   {
      [Answer(25010)]
      public object FirstPart()
      {
         return InputReader.ReadLines(this)
                           .Select(CountWinningNumbers)
                           .Select(CalcPoints)
                           .Sum();
      }

      [Answer(9924412)]
      public object SecondPart()
      {
         Dictionary<int, int> extraCards = [];

         int cardId = 0;
         foreach (var line in InputReader.ReadLines(this))
         {
            cardId++;

            int numberOfWinningNumbers = CountWinningNumbers(line);
            if (numberOfWinningNumbers == 0)
            {
               continue;
            }

            if (!extraCards.TryGetValue(cardId, out int extra))
            {
               extra = 0;
               extraCards[cardId] = 0;
            }

            foreach (var n in Enumerable.Range(cardId + 1, numberOfWinningNumbers))
            {
               extraCards[n] = extraCards.TryGetValue(n, out int current)
                  ? current + extra + 1
                  : extra + 1;
            }
         }

         int count = 0;
         for (int i = 1; i <= cardId; i++)
         {
            count += 1;
            if (extraCards.TryGetValue(i, out int extra))
            {
               count += extra;
            }
         }

         return count;
      }

      private static List<int> GetNumbers(ReadOnlySpan<char> input)
      {
         var result = new List<int>(input.Length / 3);
         for (int i = 0; i < input.Length; i += 3)
         {
            result.Add(int.Parse(input.Slice(i, 3)));
         }
         return result;
      }

      private int CalcPoints(int numberOfWinningNumbers)
      {
         if (numberOfWinningNumbers == 0)
         {
            return 0;
         }
         return (int)Math.Pow(2, numberOfWinningNumbers - 1);
      }

      private int CountWinningNumbers(string line)
      {
         var span = line.AsSpan();
         var p1 = span.IndexOf(":");
         var p2 = span.IndexOf("|");

         var winningNumbers = GetNumbers(span.Slice(p1 + 1, p2 - p1 - 2)).ToHashSet();
         var yourNumbers = GetNumbers(span.Slice(p2 + 1));
         return yourNumbers.Where(winningNumbers.Contains).Count();
      }
   }
}
