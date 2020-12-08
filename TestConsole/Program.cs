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
            await Task.Run(async () =>
            {
                await Task.Delay(5000);
                operation.Finish();
            });
            await operation;
            Console.ReadKey();
        }
    }
}
