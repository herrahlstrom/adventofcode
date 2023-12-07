using System;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2023
{
   [Puzzle(2023, 6, "Wait For It")]
   internal class Year2023Day06 : IPuzzle
   {
      [Answer(140220)]
      public object FirstPart()
      {
         var input = InputReader.ReadAllLines(this);
         int[] time = input[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
         int[] distance = input[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

         long result = 1;
         for (int i = 0; i < time.Length; i++)
         {
            result *= SimpleSolution(time[i], distance[i]);
         }
         return result;
      }

      [Answer(39570185L)]
      public object SecondPart()
      {
         var input = InputReader.ReadAllLines(this);
         long time = long.Parse(input[0][5..].Where(char.IsNumber).ToArray().AsSpan());
         long distance = long.Parse(input[1][9..].Where(char.IsNumber).ToArray().AsSpan());

         return PqEquationSolution(time, distance);
      }

      private static long PqEquationSolution(double time, double distance)
      {
         // x: time for button down

         //int left = time - x;
         //int distance = left * x;

         //int distance = (time - x) * x;

         //var distance = time * x - x * x;

         // PQ equation
         // x^2 - time * x + distance = 0

         var startingToWin = Math.Ceiling((time / 2) - Math.Sqrt(Math.Pow(time / 2, 2) - distance));
         var stoppingToWin = Math.Floor((time / 2) + Math.Sqrt(Math.Pow(time / 2, 2) - distance));

         return (long)(stoppingToWin - startingToWin) + 1;
      }

      private static long SimpleSolution(long maxTime, long winnerDistance)
      {
         int count = 0;
         for (long speedTime = 0; speedTime < maxTime; speedTime++)
         {
            long left = maxTime - speedTime;
            long distance = left * speedTime;

            if (distance > winnerDistance)
            {
               count++;
            }
         }
         return count;
      }
   }
}
