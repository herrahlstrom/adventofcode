namespace AdventOfCode.Helper;

internal static class MapExtensions
{
    public static bool OnMap<T>(this T[,] map, Point p)
    {
        return p.X >= 0 && p.X < map.GetLength(0) &&
               p.Y >= 0 && p.Y < map.GetLength(1);
    }
}