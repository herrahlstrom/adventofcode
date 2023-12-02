using CommandLine;

namespace AdventOfCode;

internal class Arguments
{
    [Option('y', "year")]
    public int? Year { get; set; }
    
    [Option('d', "day")]
    public int? Day { get; set; }
}
