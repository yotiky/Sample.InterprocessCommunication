using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a message, and then press Enter.");

            Task.Run(async () =>
            {
                while (true)
                {
                    using (var stream = new NamedPipeServerStream("NamedPipe"))
                    {
                        await stream.WaitForConnectionAsync();

                        var data = Console.ReadLine();

                        using (var writer = new StreamWriter(stream))
                        {
                            await writer.WriteLineAsync(data);
                        }
                    }
                    Thread.Sleep(100);
                }
            }).Wait();
        }
    }
}
