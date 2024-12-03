using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 3, "Mull It Over")]
public class Year2024Day03 : IPuzzle
{
    [Answer(170807108L)]
    public object FirstPart()
    {
        var span = InputReader.ReadAllText(this).AsSpan();

        long result = 0;

        for (int i = 0; i < span.Length; i++)
        {
            if (TryParseMulProduct(span, ref i, out int product))
            {
                result += product;
            }
        }

        return result;
    }

    [Answer(74838033L)]
    public object SecondPart()
    {
        var span = InputReader.ReadAllText(this).AsSpan();

        long result = 0;
        bool enabled = true;
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i..].StartsWith("do()"))
            {
                enabled = true;
                i += 3;
                continue;
            }

            if (span[i..].StartsWith("don't()"))
            {
                enabled = false;
                i += 6;
                continue;
            }

            if (enabled && TryParseMulProduct(span, ref i, out int product))
            {
                result += product;
            }
        }

        return result;
    }

    private static bool TryParseMulProduct(ReadOnlySpan<char> span, ref int i, out int product)
    {
        if (!span[i..].StartsWith("mul("))
        {
            product = default;
            return false;
        }

        i += 4;
        int state = 0;
        int a = 0, b = 0;
        for (; i < span.Length; i++)
        {
            if (state == 0 && char.IsNumber(span[i]) && a < 1000)
            {
                a = a * 10 + (int)span[i] - 48;
            }
            else if (state == 1 && char.IsNumber(span[i]) && b < 1000)
            {
                b = b * 10 + (int)span[i] - 48;
            }
            else if (state == 0 && span[i] == ',')
            {
                state = 1;
            }
            else if (state == 1 && span[i] == ')')
            {
                product = a * b;
                return true;
            }
            else
            {
                break;
            }
        }

        product = default;
        return false;
    }
}