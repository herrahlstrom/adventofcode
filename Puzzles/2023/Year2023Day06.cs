using AdventOfCode.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

         int result = 1;
         for (int i = 0; i < time.Length; i++)
         {
            //result *= SimpleSolution(time[i], distance[i]);
            result *= (int)PqEquationSolution(time[i], distance[i]);
         }
         return result;
      }

      public object SecondPart()
      {
         // 26256699 is to low
         // 53837285 is to high
         var input = InputReader.ReadAllLines(this);
         int time = int.Parse(input[0][5..].Where(char.IsNumber).ToArray().AsSpan());
         int distance = int.Parse(input[0][9..].Where(char.IsNumber).ToArray().AsSpan());

         return PqEquationSolution(time, distance);
      }

      private int SimpleSolution(int maxTime, int winnerDistance)
      {
         int count = 0;
         for (int speedTime = 0; speedTime < maxTime; speedTime++)
         {
            int left = maxTime - speedTime;
            int distance = left * speedTime;

            if (distance > winnerDistance)
            {
               count++;
            }
         }
         return count;
      }

      private double PqEquationSolution(double time, double distance)
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

         return stoppingToWin - startingToWin + 1;
      }
   }
}
