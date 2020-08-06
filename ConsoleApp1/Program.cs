using MessagePack;
using System;
using System.IO.MemoryMappedFiles;

namespace ConsoleApp1
{
    class Program
    {
		static void Main(string[] args)
        {
			//WriteMemory();
			WriteMemorySerialize();
        }
		static void WriteMemory()
		{
			Console.WriteLine("Please enter a message, and then press Enter.");

			using (var sharedMemory = MemoryMappedFile.CreateNew("SharedMemory", 1024))
			{
				while (true)
				{
					var str = Console.ReadLine();
					var data = str.ToCharArray();
					using (var accessor = sharedMemory.CreateViewAccessor())
					{
						accessor.Write(0, data.Length);
						accessor.WriteArray(sizeof(int), data, 0, data.Length);
					}
				}
			}
		}
		static void WriteMemorySerialize()
		{
			using (var sharedMemory = MemoryMappedFile.CreateNew("SharedMemory", 1024))
			{
				{
					Console.WriteLine("Please press Enter.");
					Console.ReadLine();

					var data = new User
					{
						Id = 1,
						UserId = 1,
						Name = "Shion",
						Age = 17,
					};
					var serialized = MessagePackSerializer.Serialize<IDataProtocol>(data);
					using (var accessor = sharedMemory.CreateViewAccessor())
					{
						accessor.Write(0, serialized.Length);
						accessor.WriteArray(sizeof(int), serialized, 0, serialized.Length);
					}
				}
				{
					Console.WriteLine("Please press Enter.");
					Console.ReadLine();

					var data = new Command
					{
						Id = 2,
						Name = "IncrementAge",
						UserId = 1,
					};
					var serialized = MessagePackSerializer.Serialize<IDataProtocol>(data);
					using (var accessor = sharedMemory.CreateViewAccessor())
					{
						accessor.Write(0, serialized.Length);
						accessor.WriteArray(sizeof(int), serialized, 0, serialized.Length);
					}
				}
			}
			Console.ReadLine();
		}
	}
}
