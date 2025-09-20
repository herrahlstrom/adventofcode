namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Method)]
internal class AnswerAttribute(object value) : Attribute
{
    public object Value { get; } = value;
}

[AttributeUsage(AttributeTargets.Class)]
internal class PuzzleAttribute(int year, int day, string name) : Attribute
{
    public int Day { get; } = day;

    public string Name { get; } = name;

    public int Year { get; } = year;
}