using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 11, "Plutonian Pebble")]
internal class Year2024Day11 : IPuzzle
{
    [Answer(199986L)]
    public object FirstPart()
    {
        return Calculate(25);
    }

    [Answer(236804088748754L)]
    public object SecondPart()
    {
        return Calculate(75);
    }

    private object Calculate(int numberOfBlinks)
    {
        var stones = InputReader.ReadAllText(this).Split()
            .Select(int.Parse).Select(x => new Stone(x))
            .ToList();

        for (var blink = 1; blink <= numberOfBlinks; blink++)
        {
            Dictionary<long, Stone> precalculatedStones = [];
            foreach (var stone in stones.SelectMany(x => x.GetStones()))
            {
                // Value before process/blink
                var val = stone.Value!.Value;

                if (precalculatedStones.TryGetValue(val, out var twin))
                {
                    stone.SetTwin(twin);
                }
                else
                {
                    stone.Process();
                    precalculatedStones[val] = stone;
                }
            }
        }

        return stones.Sum(x => x.CountStones());
    }

    private class Stone(long value)
    {
        private long _count;
        private Stone? _left;
        private Stone? _right;
        private Stone? _twin;
        public long? Value { get; private set; } = value;

        public long CountStones()
        {
            if (_count > 0)
                return _count;

            if (_twin != null)
                _count = _twin.CountStones();
            else if (_left != null && _right != null)
                _count = _left.CountStones() + _right.CountStones();
            else
                _count = 1;

            return _count;
        }

        public IEnumerable<Stone> GetStones()
        {
            if (Value != null)
            {
                yield return this;
            }
            else if (_left != null && _right != null)
            {
                foreach (var stone in _left.GetStones())
                {
                    yield return stone;
                }

                foreach (var stone in _right.GetStones())
                {
                    yield return stone;
                }
            }
        }

        public void Process()
        {
            if (Value is null)
                return;

            if (Value == 0)
            {
                Value = 1;
            }
            else if (TrySplit())
            {
            }
            else
            {
                Value *= 2024;
            }

            _count = 0;
        }


        public void SetTwin(Stone twinStone)
        {
            _left = null;
            _right = null;
            Value = null;

            _twin = twinStone;
        }

        private bool TrySplit()
        {
            long lower = 10;
            long upper = 99;
            long div = 10;

            while (true)
            {
                if (Value < lower)
                    return false;
                if (Value <= upper)
                {
                    _left = new Stone(Value.Value / div);
                    _right = new Stone(Value.Value % div);
                    Value = null;
                    return true;
                }

                lower *= 100;
                upper = upper * 100 + 99;
                div *= 10;
            }
        }
    }
}