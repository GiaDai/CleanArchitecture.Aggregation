namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.DependencyInjection
{
    /// <summary>
    /// Method Injection là cách tiếp cận mà các dependency được tiêm vào thông qua các phương thức của lớp
    /// NET Core cũng hỗ trợ Method Injection, nhưng cũng không được sử dụng phổ biến như Constructor Injection vì cần phải gọi phương thức tiêm dependency mỗi khi cần sử dụng.
    /// </summary>
    public interface IMethodInjectionService
    {
        void PrintMessage();
    }

    public class MethodInjectionService : IMethodInjectionService
    {
        public void PrintMessage()
        {
            Console.WriteLine("Hello from MethodInjectionService");
        }
    }

    public class MethodInjection
    {
        public void Run(IMethodInjectionService service)
        {
            service.PrintMessage();
        }
    }

    public static class RunMethodInjection
    {
        public static void Run()
        {
            MethodInjection methodInjection = new MethodInjection();
            IMethodInjectionService service = new MethodInjectionService();
            methodInjection.Run(service);
        }
    }
}
