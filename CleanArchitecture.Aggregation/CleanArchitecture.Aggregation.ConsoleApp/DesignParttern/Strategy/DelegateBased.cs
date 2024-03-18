using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Strategy
{
    /// <summary>
    /// Trong cách triển khai này, chúng ta sử dụng delegate để đại diện cho các hành vi (thuật toán) và có thể hoán đổi delegate tại thời điểm runtime
    /// Điều này cho phép chúng ta truyền các hành vi vào lớp chính thông qua delegate, giúp tạo ra sự linh hoạt và dễ bảo trì.
    /// </summary>
    
    // Delegate chung cho các thuật toán
    public delegate void Algorithm();

    // Lớp chính sử dụng Strategy Pattern
    public class  DelegateBasedContext
    {
        private Algorithm _algorithm;

        public void SetAlgorithm(Algorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public void ExecuteAlgorithm()
        {
            _algorithm?.Invoke();
        }
    }

    // Run Strategy Pattern
    public class RunDelegateBased
    {
        public static void Run()
        {
            DelegateBasedContext context = new DelegateBasedContext();

            // Set thuật toán A
            context.SetAlgorithm(AlgorithmA);
            context.ExecuteAlgorithm();

            // Set thuật toán B
            context.SetAlgorithm(AlgorithmB);
            context.ExecuteAlgorithm();
        }

        private static void AlgorithmA()
        {
            Console.WriteLine("AlgorithmA()");
        }

        private static void AlgorithmB()
        {
            Console.WriteLine("AlgorithmB()");
        }
    }
}
