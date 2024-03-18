using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Strategy Pattern là một design pattern quan trọng được sử dụng để định nghĩa một tập hợp các thuật toán và làm cho chúng 
/// có thể hoán đổi được với nhau tại thời điểm runtime mà không cần thay đổi code của lớp sử dụng thuật toán đó. 
/// Strategy Pattern giúp tách biệt logic của thuật toán ra khỏi code chính của ứng dụng, tăng tính linh hoạt và dễ bảo trì của mã nguồn
/// </summary>
namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Strategy
{
    /// <summary>
    /// Trong cách triển khai này, chúng ta định nghĩa một interface chung cho các thuật toán và triển khai các lớp thuật toán khác nhau dựa trên interface đó.
    /// Lớp chính sử dụng Strategy Pattern sẽ có một thuộc tính hoặc phương thức để thiết lập và thay đổi thuật toán tại runtime thông qua giao diện đã định nghĩa.
    /// </summary>
    

    // Inerface chung cho các thuật toán
    public interface IStrategy
    {
        void Algorithm();
    }

    // Các lớp thuật toán khác nhau
    public class ConcreteStrategyA : IStrategy
    {
        public void Algorithm()
        {
            Console.WriteLine("ConcreteStrategyA.Algorithm()");
        }
    }

    public class ConcreteStrategyB : IStrategy
    {
        public void Algorithm()
        {
            Console.WriteLine("ConcreteStrategyB.Algorithm()");
        }
    }

    // Lớp chính sử dụng Strategy Pattern
    public class Context
    {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void ExecuteStrategy()
        {
            _strategy.Algorithm();
        }
    }

    // Run Strategy Pattern
    public class RunInterfaceBased
    {
        public static void Run()
        {
            Context context = new Context();

            // Set thuật toán A
            context.SetStrategy(new ConcreteStrategyA());
            context.ExecuteStrategy();

            // Set thuật toán B
            context.SetStrategy(new ConcreteStrategyB());
            context.ExecuteStrategy();
        }
    }
}
