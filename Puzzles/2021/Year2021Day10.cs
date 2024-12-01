using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 10, "Syntax Scoring")]
internal class Year2021Day10 : IPuzzle
{
    private IList<string> _lines = Array.Empty<string>();

    public object FirstPart()
    {
        ReadInput();

        long score = 0;
        foreach (string line in _lines)
        {
            try
            {
                ProcessLine(line);
            }
            catch (CorruptedChunkException ex)
            {
                score += ex.InvalidCharacter switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => throw new InvalidOperationException()
                };
            }
            catch (IncompleteChunkException) { }
        }

        return score;
    }

    public void ReadInput()
    {
        _lines = InputReader.ReadAllLines(this);
    }

    public object SecondPart()
    {
        List<long> scores = new();
        foreach (string line in _lines)
        {
            try
            {
                ProcessLine(line);
            }
            catch (CorruptedChunkException) { }
            catch (IncompleteChunkException ex)
            {
                long score = 0;
                foreach (char c in ex.Remains)
                {
                    score *= 5;
                    score += c switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                        _ => throw new InvalidOperationException()
                    };
                }

                scores.Add(score);
            }
        }

        return scores.OrderBy(x => x).Skip((scores.Count - 1) / 2).First();
    }

    private static void ProcessLine(string line)
    {
        Stack<char> chunkStack = new();

        foreach (char c in line)
        {
            switch (c)
            {
                case '(':
                case '[':
                case '{':
                case '<':
                    chunkStack.Push(c);
                    continue;
            }

            char expecting = c switch
            {
                ')' => '(',
                ']' => '[',
                '}' => '{',
                '>' => '<',
                _ => throw new InvalidOperationException($"Invaid character: {c}")
            };

            if (chunkStack.Count == 0 || chunkStack.Pop() != expecting)
            {
                throw new CorruptedChunkException(c);
            }
        }

        if (chunkStack.Count > 0)
        {
            throw new IncompleteChunkException(string.Join("", chunkStack.ToArray()));
        }
    }

    private class CorruptedChunkException : Exception
    {
        public CorruptedChunkException(char invalidCharacter)
        {
            InvalidCharacter = invalidCharacter;
        }

        public char InvalidCharacter { get; }
    }

    private class IncompleteChunkException : Exception
    {
        public IncompleteChunkException(string remains)
        {
            Remains = remains;
        }

        public string Remains { get; }
    }
}