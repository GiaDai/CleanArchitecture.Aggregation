namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.DependencyInjection
{
    /// <summary>
    /// Constructor Injection là cách tiếp cận phổ biến nhất trong DI, nơi các dependency được truyền vào thông qua các tham số của constructor của lớp.
    /// .NET Core sử dụng Constructor Injection để tiêm dependency vào các thành phần như các service, controller, view components, middleware, etc.
    /// </summary>
    public interface IConstructorInjectionService
    {
        void PrintMessage();
    }

    public class ConstructorInjectionService : IConstructorInjectionService
    {
        public void PrintMessage()
        {
            Console.WriteLine("Hello from ConstructorInjectionService");
        }
    }

    public class ConstructorInjection
    {
        private readonly IConstructorInjectionService _service;

        public ConstructorInjection(IConstructorInjectionService service)
        {
            _service = service;
        }

        public void Run()
        {
            _service.PrintMessage();
        }
    }

    public static class RunContructorInjection
    {
        public static void Run()
        {
            var service = new ConstructorInjectionService();
            var constructorInjection = new ConstructorInjection(service);
            constructorInjection.Run();
        }
    }

}