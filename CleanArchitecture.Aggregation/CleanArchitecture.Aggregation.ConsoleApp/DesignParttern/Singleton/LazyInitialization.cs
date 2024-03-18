using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  Ý tưởng chính vẫn là đảm bảo rằng một lớp chỉ có duy nhất một thể hiện và cung cấp một cách để truy cập global đến thể hiện đó
///  </summary>
namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Singleton
{
    /// <summary>
    /// Trong Lazy Initialization Singleton, thể hiện của lớp được tạo ra chỉ khi nó được yêu cầu lần đầu tiên, không phải làm cho ứng dụng tốn tài nguyên mà không cần thiết.
    /// Sử dụng Lazy<T> trong .NET để đảm bảo rằng thể hiện được tạo ra chỉ khi cần thiết và đảm bảo luồng an toàn (thread-safe) khi sử dụng.
    /// </summary>
    public class LazyInitialization
    {
        private static readonly Lazy<LazyInitialization> _instance = new Lazy<LazyInitialization>(() => new LazyInitialization());
        public static LazyInitialization Instance => _instance.Value;

        private LazyInitialization()
        {
            Console.WriteLine("LazyInitialization instance created");
        }

        public void PrintDetails(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class RunLazyInitialization
    {
        public static void Run()
        {
            // Try cập Singleton thông qua Instance property
            LazyInitialization instanceOne = LazyInitialization.Instance;
            instanceOne.PrintDetails("LazyInitialization instanceOne");

            LazyInitialization instanceTwo = LazyInitialization.Instance;
            instanceTwo.PrintDetails("LazyInitialization instanceTwo");

            // instanceOne và instanceTwo là cùng một thể hiện và trỏ đến cùng một địa chỉ bộ nhớ
            Console.WriteLine($"instanceOne == instanceTwo: {instanceOne == instanceTwo}");
        }
    }
}
