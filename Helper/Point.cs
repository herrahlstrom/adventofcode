using System.Diagnostics;

namespace AdventOfCode.Helper;

[DebuggerDisplay("{X},{Y}")]
internal struct Point(int x, int y)
{

    public static readonly Point Up = new(0, -1);
    public static readonly Point Right = new(1, 0);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);

    public int X { get; } = x;

    public int Y { get; } = y;

    public static bool operator ==(Point a, Point b) => a.Equals(b);

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static bool operator !=(Point a, Point b) => !a.Equals(b);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

    public override bool Equals(object? obj)
    {
        return obj is Point other && X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public readonly Point MoveDown() => this + Down;

    public readonly Point MoveLeft() => this + Left;

    public readonly Point MoveRight() => this + Right;

    public readonly Point MoveUp() => this + Up;
    public override string ToString()
    {
        return $"{{{X},{Y}}}";
    }
}
