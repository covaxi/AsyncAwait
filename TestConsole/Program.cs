using Library;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    using LongOperation = LongOperation<int>;

    class Program
    {
        static async Task Main(string[] args)
        {
            var operation = new LongOperation(() =>
            {
                Thread.Sleep(2000);
                return 0;
            });
            operation.Log = s => Console.WriteLine(s);
            await operation.StartAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
