using BenchmarkDotNet.Running;

namespace DotVast.Benchmark.Reflection;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<ReadProperty>();
        //ReadProperty readProperty = new();
        //Console.WriteLine(readProperty.ReadPub());
        //Console.WriteLine(readProperty.ReadPriByReflection());
        //Console.WriteLine(readProperty.ReadPriByCachedReflection());
        //Console.WriteLine(readProperty.ReadPriByDelegate());
        //Console.WriteLine(readProperty.ReadPriByCachedDelegate());
        //Console.WriteLine(readProperty.ReadPriByExpression());
        //Console.WriteLine(readProperty.ReadPriByCachedExpression());
    }
}
