using System;
using System.Diagnostics;
using System.Linq;
using DiffTool.Ancillary;
using DiffTool.Core;

namespace StarterApp
{
    internal class Program
    {
        private static readonly string _alph = "qwertyuiop[]asdfghjkl;'zxcvbnm,.";
        private static readonly Random _random = new Random();

        static void Main(string[] args)
        {
            int rearrangesBenchmark = 0;
            int changedLinesProcessorBenchmark = 1;

            if (rearrangesBenchmark == 1) RearrangesBenchmark();

            if (changedLinesProcessorBenchmark == 1) ChangedLinesProcessorBenchmark();

            Console.ReadKey();
        }

        private static void RearrangesBenchmark()
        {
            var rearranger = new SimpleRearranger();
            var stopwatch = Stopwatch.StartNew();
            rearranger.GetRearranges(32, 20, rearrange => { });
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private static void ChangedLinesProcessorBenchmark()
        {
            var linesCount = 15;
            var oldText = new Text(String.Join("\n", Enumerable.Range(0, linesCount).Select(_ => GetRandomString(100))));
            var newText = new Text(String.Join("\n", Enumerable.Range(0, 2 * linesCount).Select(_ => GetRandomString(100))));
            var processor = new ChangedLinesProcessor();
            var stopwatch = Stopwatch.StartNew();
            var result = processor.GetResult(0, linesCount, oldText, 0, 2 * linesCount, newText);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        private static string GetRandomString(int length)
        {
            var buf = new char[length];
            for (int i = 0; i < length; i++)
            {
                buf[i] = _alph[_random.Next(_alph.Length)];
            }

            return new string(buf);
        }
    }
}
