using ConsoleApp1;
using MessagePack;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace ConsoleApp2
{
	class Program
	{
		static void Main(string[] args)
        {
			//ReadMemory();
			ReadMemoryDeserialize();
		}
		static void ReadMemory()
		{
			using (var sharedMemory = MemoryMappedFile.OpenExisting("SharedMemory"))
			{
				var latestMsg = string.Empty;
				while (true)
				{
					using (var accessor = sharedMemory.CreateViewAccessor())
					{
						var size = accessor.ReadInt32(0);
						var data = new char[size];
						accessor.ReadArray<char>(sizeof(int), data, 0, data.Length);
						var str = new string(data);
						if (latestMsg != str)
						{
							latestMsg = str;
							Console.WriteLine("Data :" + str);
						}
					}

					Thread.Sleep(100);
				}
			}
		}

		static void ReadMemoryDeserialize()
		{
			User user = null;
			using (var sharedMemory = MemoryMappedFile.OpenExisting("SharedMemory"))
			{
				var state = 0;
				while (state < 2)
				{
					using (var accessor = sharedMemory.CreateViewAccessor())
					{
						var size = accessor.ReadInt32(0);
						if (0 < size)
						{
							var data = new byte[size];
							accessor.ReadArray<byte>(sizeof(int), data, 0, data.Length);
							var deserialized = MessagePackSerializer.Deserialize<IDataProtocol>(data);
							switch (deserialized)
							{
								case User x:
									if (state < x.Id)
									{
										Console.WriteLine(MessagePackSerializer.SerializeToJson(x));
										user = x;
										state = x.Id;
									}
									break;
								case Command x:
									if (state < x.Id)
									{
										Console.WriteLine(MessagePackSerializer.SerializeToJson(x));
										if (x.Name == "IncrementAge")
											if (user?.UserId == x.UserId)
												user.IncrementAge();
										Console.WriteLine(MessagePackSerializer.SerializeToJson(user));
										state = x.Id;
									}
									break;
								default:
									break;
							}
						}
					}

					Thread.Sleep(100);
				}
			}
		}
	}
}