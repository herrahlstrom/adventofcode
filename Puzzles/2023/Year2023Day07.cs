using System;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles
{
   [Puzzle(2023, 7, "Camel Cards")]
   public class Year2023Day07 : IPuzzle
   {
      private enum HandType
      {
         FiveOfAKind = 7,
         FourOfAKind = 6,
         FullHouse = 5,
         ThreeOfAKind = 4,
         TwoPair = 3,
         OnePair = 2,
         HighCard = 1
      }

      [Answer(251545216)]
      public object FirstPart()
      {
         Hand.Part = 1;

         var hands = InputReader.ReadLines(this)
                                .Select(ReadHand)
                                .ToList();
         hands.Sort();

         return hands.Select((a, i) => (i + 1) * a.Betting)
                     .Sum();
      }

      [Answer(250384185)]
      public object SecondPart()
      {
         Hand.Part = 2;

         var hands = InputReader.ReadLines(this)
                                .Select(ReadHand)
                                .ToList();
         hands.Sort();

         return hands.Select((a, i) => (i + 1) * a.Betting)
                     .Sum();
      }

      private static Hand ReadHand(string line)
      {
         var cards = line.Take(5).ToArray();
         int betting = int.Parse(line[6..]);
         return new Hand(cards, betting);
      }

      private class Hand(char[] cards, int betting) : IComparable<Hand>
      {
         public static int Part { get; set; }

         public int Betting { get; } = betting;

         public int[] CardPoints { get; } = cards.Select(GetCardPoint).ToArray();

         public HandType Type { get; } = GetType(cards);

         public int CompareTo(Hand? other)
         {
            ArgumentNullException.ThrowIfNull(other);
            var d = this.Type.CompareTo(other.Type);
            if (d != 0)
            {
               return d;
            }
            for (int i = 0; i < 5; i++)
            {
               d = this.CardPoints[i].CompareTo(other.CardPoints[i]);
               if (d != 0)
               {
                  return d;
               }
            }
            return 0;
         }

         private static int GetCardPoint(char c) => c switch
         {
            'J' when (Part == 2) => 0,
            '2' => 1,
            '3' => 2,
            '4' => 3,
            '5' => 4,
            '6' => 5,
            '7' => 6,
            '8' => 7,
            '9' => 8,
            'T' => 9,
            'J' when (Part == 1) => 10,
            'Q' => 11,
            'K' => 12,
            'A' => 13,
            _ => throw new ArgumentOutOfRangeException(nameof(c))
         };

         static HandType GetType(char[] cards)
         {
            int[] perValue;

            switch (Part)
            {
               case 1:
                  perValue = cards.GroupBy(x => x)
                                  .Select(x => x.Count())
                                  .OrderByDescending(x => x)
                                  .ToArray();
                  break;

               case 2:
                  int numberOfJokers = cards.Where(c => c == 'J').Count();
                  perValue = cards.Where(c => c != 'J')
                                  .GroupBy(x => x)
                                  .Select(x => x.Count() + numberOfJokers)
                                  .OrderByDescending(x => x)
                                  .ToArray();
                  break;

               default:
                  throw new ArgumentOutOfRangeException(nameof(cards));
            }

            return perValue.Length switch
            {
               0 when Part.Equals(2) => HandType.FiveOfAKind,

               1 => HandType.FiveOfAKind,

               2 => perValue[0] switch
               {
                  4 => HandType.FourOfAKind,
                  3 => HandType.FullHouse,
                  _ => throw new ArgumentOutOfRangeException(nameof(cards)),
               },

               3 => perValue[0] switch
               {
                  3 => HandType.ThreeOfAKind,
                  2 => HandType.TwoPair,
                  _ => throw new ArgumentOutOfRangeException(nameof(cards)),
               },

               4 => HandType.OnePair,

               5 => HandType.HighCard,

               _ => throw new ArgumentOutOfRangeException(nameof(cards)),
            };
         }
      }
   }
}
