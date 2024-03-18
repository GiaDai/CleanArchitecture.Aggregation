using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Singleton
{
    /// <summary>
    /// Trong Eager Initialization Singleton, thể hiện của lớp được tạo ra ngay từ đầu, ngay cả khi không có ai sử dụng nó. 
    /// Điều này có thể tiêu tốn tài nguyên hơn, nhưng đảm bảo rằng thể hiện đã sẵn sàng để sử dụng khi cần.
    // / </summary>
    public class EagerInitialization
    {
        private static readonly EagerInitialization _instance = new EagerInitialization();
        public static EagerInitialization Instance => _instance;

        private EagerInitialization()
        {
            Console.WriteLine("EagerInitialization instance created");
        }

        public void PrintDetails(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class RunEagerInitialization
    {
        public static void Run()
        {
            // Try cập Singleton thông qua Instance property
            EagerInitialization instanceOne = EagerInitialization.Instance;
            instanceOne.PrintDetails("EagerInitialization instanceOne");

            EagerInitialization instanceTwo = EagerInitialization.Instance;
            instanceTwo.PrintDetails("EagerInitialization instanceTwo");

            // instanceOne và instanceTwo là cùng một thể hiện và trỏ đến cùng một địa chỉ bộ nhớ
            Console.WriteLine($"instanceOne == instanceTwo: {instanceOne == instanceTwo}");
        }
    }
}
