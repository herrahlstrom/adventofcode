using System.Diagnostics;

namespace AdventOfCode;

[DebuggerDisplay("{X},{Y}")]
internal struct Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static bool operator ==(Point a, Point b) => a.Equals(b);
    public static bool operator !=(Point a, Point b) => !a.Equals(b);

    public static readonly Point Up = new(0, -1);
    public static readonly Point Right = new(1, 0);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);

    public override bool Equals(object? obj)
    {
        return obj is Point other && X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}