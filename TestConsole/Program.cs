using Library;
using System;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var operation = new LongOperation();
            operation.Log = s => Console.WriteLine(s);
            await operation.StartAsync();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
