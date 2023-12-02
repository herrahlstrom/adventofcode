using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 18, "Snailfish")]
internal class Year2021Day18 : IPuzzle
{
    private string[] _snailfishLines = Array.Empty<string>();

    public object FirstPart()
    {
        ReadInput();

        SnailfishNode result = GetSnailfishNode(_snailfishLines.First());

        foreach (string snailfishLine in _snailfishLines.Skip(1))
        {
            var newPair = new SnailfishNode
            {
                Left = result,
                Right = GetSnailfishNode(snailfishLine)
            };
            newPair.Left.Parent = newPair;
            newPair.Right.Parent = newPair;
            result = newPair;

            Reduce(result);
        }

        return result.GetMagnitude();
    }

    public void ReadInput()
    {
        _snailfishLines = InputReader.ReadAllLines(this);
    }

    public object SecondPart()
    {
        int best = 0;


        for (int i = 0; i < _snailfishLines.Length; i++)
        {
            for (int j = 0; j < _snailfishLines.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var p = new SnailfishNode
                {
                    Left = GetSnailfishNode(_snailfishLines[i]),
                    Right = GetSnailfishNode(_snailfishLines[j])
                };
                p.Left.Parent = p;
                p.Right.Parent = p;

                Reduce(p);

                int result = p.GetMagnitude();
                if (result > best)
                {
                    best = result;
                }
            }
        }

        return best;
    }

    private static void Explode(SnailfishNode node)
    {
        if (node.Level <= 4)
        {
            return;
        }

        if (node.Left is { IsPair: true } pairOfLeft)
        {
            Explode(pairOfLeft);
            return;
        }

        if (node.Right is { IsPair: true } pairOfRight)
        {
            Explode(pairOfRight);
            return;
        }


        if (node.Left is { Value: { } leftValue }) { }
        else
        {
            throw new NotSupportedException();
        }

        if (node.Right is { Value: { } rightValue }) { }
        else
        {
            throw new NotSupportedException();
        }

        if (node.Parent is not { } parent)
        {
            throw new NotSupportedException();
        }

        SnailfishNode? nextOnLeft;
        SnailfishNode? nextOnRight;

        node.Left = null;
        node.Right = null;
        node.Value = 0;

        nextOnLeft = FindNextOnLeft(parent, node);
        nextOnRight = FindNextOnRight(parent, node);

        if (nextOnLeft is not null)
        {
            nextOnLeft.Value += leftValue;
        }

        if (nextOnRight is not null)
        {
            nextOnRight.Value += rightValue;
        }
    }

    private static SnailfishNode? FindNextOnLeft(SnailfishNode? node, SnailfishNode from)
    {
        if (node is null)
        {
            return null;
        }

        if (node.HasValue)
        {
            return node;
        }

        if (node.IsPair && node.Left is not null && node.Right is not null)
        {
            if (node.Left.Equals(from))
            {
                return FindNextOnLeft(node.Parent, node);
            }

            if (node.Right.Equals(from))
            {
                return FindNextOnLeft(node.Left, node) ??
                       FindNextOnLeft(node.Parent, node);
            }

            if (node.Parent?.Equals(from) == true)
            {
                return FindNextOnLeft(node.Right, node);
            }

            throw new NotSupportedException();
        }

        throw new NotSupportedException();
    }

    private static SnailfishNode? FindNextOnRight(SnailfishNode? node, SnailfishNode from)
    {
        if (node is null)
        {
            return null;
        }

        if (node.HasValue)
        {
            return node;
        }

        if (node.IsPair && node.Left is not null && node.Right is not null)
        {
            if (node.Left.Equals(from))
            {
                return FindNextOnRight(node.Right, node) ??
                       FindNextOnRight(node.Parent, node);
            }

            if (node.Right.Equals(from))
            {
                return FindNextOnRight(node.Parent, node);
            }

            if (node.Parent?.Equals(from) == true)
            {
                return FindNextOnRight(node.Left, node);
            }

            throw new NotSupportedException();
        }

        throw new NotSupportedException();
    }

    private static IEnumerable<SnailfishNode> GetNumbersFromLeft(SnailfishNode node)
    {
        yield return node;

        if (node.IsPair && node.Left is not null && node.Right is not null)
        {
            foreach (SnailfishNode x in GetNumbersFromLeft(node.Left))
            {
                yield return x;
            }

            foreach (SnailfishNode x in GetNumbersFromLeft(node.Right))
            {
                yield return x;
            }
        }
    }

    private static SnailfishNode GetSnailfishNode(ReadOnlySpan<char> line)
    {
        if (char.IsNumber(line[0]) && int.TryParse(line, out int value))
        {
            return new SnailfishNode { Value = value };
        }

        if (line[0] != '[' || line[^1] != ']')
        {
            throw new NotSupportedException();
        }

        List<SnailfishNode> result = new(2);

        int bCounter = 0;
        int contentStart = 1;
        for (int i = 1; i < line.Length - 1; i++)
        {
            char c = line[i];

            if (c == '[')
            {
                bCounter++;
                continue;
            }

            if (c == ']')
            {
                bCounter--;
                continue;
            }

            if (bCounter > 0)
            {
                continue;
            }

            if (bCounter < 0)
            {
                throw new NotSupportedException();
            }

            if (c == ',')
            {
                result.Add(GetSnailfishNode(line.Slice(contentStart, i - contentStart)));

                contentStart = i + 1;
            }
        }

        result.Add(GetSnailfishNode(line.Slice(contentStart, line.Length - 1 - contentStart)));


        if (result.Count != 2)
        {
            throw new NotSupportedException();
        }

        var resVal = new SnailfishNode
        {
            Left = result[0],
            Right = result[1]
        };

        SetParent(resVal, null);

        return resVal;
    }

    private static void SetParent(SnailfishNode node, SnailfishNode? parent)
    {
        node.Parent = parent;
        if (node.IsPair && node.Left is not null && node.Right is not null)
        {
            SetParent(node.Left, node);
            SetParent(node.Right, node);
        }
    }

    private void Reduce(SnailfishNode node)
    {
        bool done = false;
        while (!done)
        {
            done = true;

            SnailfishNode? needExplode = GetNumbersFromLeft(node)
                .Where(x => x.IsPair)
                .FirstOrDefault(x => x.Level > 4);
            if (needExplode != null)
            {
                Explode(needExplode);
                done = false;
                continue;
            }

            SnailfishNode? needSplit = GetNumbersFromLeft(node).Where(x => x.HasValue).FirstOrDefault(x => x.Value > 9);
            if (needSplit != null)
            {
                Split(needSplit);
                done = false;
            }
        }
    }

    private void Split(SnailfishNode node)
    {
        if (node.Value is { } value)
        {
            if (value < 10)
            {
                return;
            }
        }
        else
        {
            return;
        }

        node.Left = new SnailfishNode { Parent = node, Value = (int)Math.Floor(value / 2m) };
        node.Right = new SnailfishNode { Parent = node, Value = (int)Math.Ceiling(value / 2m) };
        node.Value = null;

        if (node.Level > 4)
        {
            Explode(node);
        }
    }

    private class SnailfishNode
    {
        public bool HasValue => Value.HasValue;
        public bool IsPair => Value is null;
        public SnailfishNode? Left { get; set; }
        public int Level => Parent is { } parent ? parent.Level + 1 : 1;
        public SnailfishNode? Parent { get; set; }
        public SnailfishNode? Right { get; set; }
        public int? Value { get; set; }

        public int GetMagnitude()
        {
            if (Value is { } v)
            {
                return v;
            }

            if (Left is { } l && Right is { } r)
            {
                return 3 * l.GetMagnitude() + 2 * r.GetMagnitude();
            }

            throw new NotSupportedException();
        }

        public override string ToString()
        {
            if (Value is { } v)
            {
                return v.ToString();
            }

            if (Left is { } l && Right is { } r)
            {
                return $"[{l},{r}]";
            }

            throw new NotSupportedException();
        }
    }
}