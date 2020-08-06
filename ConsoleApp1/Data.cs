using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    [Union(0, typeof(User))]
    [Union(1, typeof(Command))]
    public interface IDataProtocol
    {
        [Key(0)]
        int Id { get; set; }
    }

    [MessagePackObject]
    public class User : IDataProtocol
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public int UserId { get; set; }
        [Key(2)]
        public string Name { get; set; }
        [Key(3)]
        public int Age { get; set; }

        public void IncrementAge()
        {
            Age++;
        }
    }
    [MessagePackObject]
    public class Command : IDataProtocol
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public int UserId { get; set; }
    }
}
