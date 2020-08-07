using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                using (var stream = new NamedPipeClientStream("NamedPipe"))
                {
                    await stream.ConnectAsync();

                    using (var reader = new StreamReader(stream))
                    {
                        while (stream.IsConnected)
                        {
                            var str = await reader.ReadLineAsync();
                            Console.WriteLine("Data :" + str);
                            Thread.Sleep(100);
                        }
                    }
                }
            }).Wait();
        }
    }
}
