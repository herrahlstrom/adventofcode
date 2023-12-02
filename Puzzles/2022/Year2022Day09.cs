using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 9, "Rope Bridge")]
internal class Year2022Day09 : IPuzzle
{
    public object FirstPart()
    {
        Point head = new(0, 0);
        Point tail = new(0, 0);
        var visitedByTail = new HashSet<Point>();
        visitedByTail.Add(tail);

        foreach (var line in InputReader.ReadLines(this))
        {
            var dir = line[0];
            var steps = int.Parse(line[2..]);

            for (int i = 0; i < steps; i++)
            {
                head = MoveHead(head, dir);

                tail = TailCatch(head, tail);
                visitedByTail.Add(tail);
            }
        }
        return visitedByTail.Count;
    }

    public object SecondPart()
    {
        Point head = new(0, 0);
        Point[] tails = Enumerable.Repeat(new Point(0, 0), 9).ToArray();
        var visitedByTail = new HashSet<Point>();
        visitedByTail.Add(tails[8]);

        foreach (var line in InputReader.ReadLines(this))
        {
            var dir = line[0];
            var steps = int.Parse(line[2..]);

            for (int i = 0; i < steps; i++)
            {
                head = MoveHead(head, dir);

                tails[0] = TailCatch(head, tails[0]);
                tails[1] = TailCatch(tails[0], tails[1]);
                tails[2] = TailCatch(tails[1], tails[2]);
                tails[3] = TailCatch(tails[2], tails[3]);
                tails[4] = TailCatch(tails[3], tails[4]);
                tails[5] = TailCatch(tails[4], tails[5]);
                tails[6] = TailCatch(tails[5], tails[6]);
                tails[7] = TailCatch(tails[6], tails[7]);
                tails[8] = TailCatch(tails[7], tails[8]);
                visitedByTail.Add(tails[8]);
            }
        }
        return visitedByTail.Count;
    }

    private static Point MoveHead(Point head, char dir)
    {
        return dir switch
        {
            'U' => head with { Y = head.Y + 1 },
            'D' => head with { Y = head.Y - 1 },
            'L' => head with { X = head.X - 1 },
            'R' => head with { X = head.X + 1 },
            _ => throw new NotSupportedException(),
        };
    }

    private static Point TailCatch(Point head, Point tail)
    {
        var deltaX = head.X - tail.X;
        var deltaY = head.Y - tail.Y;
        if (deltaX < -1 || deltaX > 1 || deltaY < -1 || deltaY > 1)
        {
            return tail with
            {
                X = tail.X + Math.Sign(deltaX),
                Y = tail.Y + Math.Sign(deltaY)
            };
        }

        return tail;
    }

    record Point(int X, int Y);
}
