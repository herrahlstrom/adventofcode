using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2023, 1, "Trebuchet?!")]
public class Year2023Day01 : IPuzzle
{
   public object FirstPart()
   {
      long sum = 0;

      foreach(string line in InputReader.ReadLines(this))
      {
         int a = line.Where(char.IsNumber).First() - 48;
         int b = line.ReadFromEnd().Where(char.IsNumber).First() - 48;
         sum += (a * 10) + b;
      }

      return sum;
   }

   public object SecondPart()
   {
      long sum = 0;

      foreach(var line in InputReader.ReadLines(this))
      {
         var span = line.AsSpan();

         int a = default;
         for(int i = 0; i < line.Length; i++)
         {
            if(TryGetValue(span[i..], out int value))
            {
               a = value;
               break;
            }
         }

         int b = default;
         for(int i = line.Length - 1; i >= 0; i--)
         {
            if(TryGetValue(span[i..], out int value))
            {
               b = value;
               break;
            }
         }

         sum += (a * 10) + b;
      }

      return sum;

      static bool TryGetValue(ReadOnlySpan<char> span, out int value)
      {
         if(char.IsNumber(span[0]))
         {
            value = span[0] - 48;
         }
         else if(span.StartsWith("one"))
         {
            value = 1;
         }
         else if(span.StartsWith("two"))
         {
            value = 2;
         }
         else if(span.StartsWith("three"))
         {
            value = 3;
         }
         else if(span.StartsWith("four"))
         {
            value = 4;
         }
         else if(span.StartsWith("five"))
         {
            value = 5;
         }
         else if(span.StartsWith("six"))
         {
            value = 6;
         }
         else if(span.StartsWith("seven"))
         {
            value = 7;
         }
         else if(span.StartsWith("eight"))
         {
            value = 8;
         }
         else if(span.StartsWith("nine"))
         {
            value = 9;
         }
         else
         {
            value = default;
            return false;
         }
         return true;
      }
   }
}
