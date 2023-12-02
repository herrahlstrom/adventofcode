using AdventOfCode.Helper;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 7, "No Space Left On Device")]
internal partial class Year2022Day07 : IPuzzle
{
    public object FirstPart()
    {
        var allDirs = ImportFileSystem().GetDescendingDirectories();

        long result = 0;
        foreach (var dir in allDirs)
        {
            var size = dir.TotalSize();
            if (size > 100000)
            {
                continue;
            }

            result += size;
        }
        return result;
    }

    public object SecondPart()
    {
        const long diskSpace = 70000000;
        const long updateSize = 30000000;

        ElvDirectory root = ImportFileSystem();
        long usedDiskSpace = root.TotalSize();
        long neededDiskSpace = usedDiskSpace + updateSize - diskSpace;

        var allDirs = root.GetDescendingDirectories();
        ElvDirectory? bestDirectory = null;
        long bestDirectorySize = long.MaxValue;

        foreach (var dir in allDirs)
        {
            var size = dir.TotalSize();
            if (size < neededDiskSpace)
            {
                continue;
            }

            if (bestDirectory is null || size < bestDirectorySize)
            {
                bestDirectory = dir;
                bestDirectorySize = size;
            }
        }
        return bestDirectorySize;
    }

    [GeneratedRegex("^dir (?<name>.+)$")]
    private static partial Regex DirNameRegex();

    [GeneratedRegex("^(?<size>\\d+) (?<name>.+)$")]
    private static partial Regex FileNameRegex();

    private ElvDirectory ImportFileSystem()
    {
        ElvDirectory root = new ElvDirectory() { Parent = null, Name = "/" };
        ElvDirectory currentDirectory = root;

        foreach (var line in InputReader.ReadLines(this))
        {
            if (line == "$ cd /")
            {
                currentDirectory = root;
            }
            else if (line == "$ cd ..")
            {
                currentDirectory = currentDirectory.Parent!;
            }
            else if (line.StartsWith("$ cd"))
            {
                currentDirectory = currentDirectory.GetDirectory(line[5..]);
            }
            else if (line == "$ ls")
            {
            }
            else if (FileNameRegex().Match(line) is { Success: true } fileMatch)
            {
                long size = long.Parse(fileMatch.Groups["size"].Value);
                //string name = fileMatch.Groups["name"].Value;
                currentDirectory.Size += size;
            }
            else if (DirNameRegex().Match(line) is { Success: true } dirMatch)
            {
                //string name = dirMatch.Groups["name"].Value;
            }
            else
            {
                throw new InvalidOperationException($"Invalid input line: {line}");
            }
        }

        return root;
    }
}

class ElvDirectory
{
    List<ElvDirectory> m_subDirectories = new();

    public required string Name { get; init; }
    public required ElvDirectory? Parent { get; init; }
    public long Size { get; set; } = 0;

    public IEnumerable<ElvDirectory> GetDescendingDirectories()
    {
        return m_subDirectories.SelectMany(x => x.GetDescendingDirectories()).Append(this);
    }

    public ElvDirectory GetDirectory(string name)
    {
        var dir = m_subDirectories.FirstOrDefault(x => x.Name == name);
        if (dir is null)
        {
            dir = new ElvDirectory() { Parent = this, Name = name };
            m_subDirectories.Add(dir);
        }
        return dir;
    }

    public long TotalSize() => Size + m_subDirectories.Sum(x => x.TotalSize());
}
