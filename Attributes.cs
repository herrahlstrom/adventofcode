using System;
using System.Linq;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Method)]
internal class AnswerAttribute : Attribute
{
   public AnswerAttribute(object value)
   {
      Value = value;
   }

   public object Value { get; }
}

[AttributeUsage(AttributeTargets.Class)]
internal class PuzzleAttribute : Attribute
{
   public PuzzleAttribute(int year, int day, string name)
   {
      Name = name;
      Year = year;
      Day = day;
   }

   public int Day { get; }

   public string Name { get; }

   public bool PrintToConsole { get; set; }

   public int Year { get; }
}
