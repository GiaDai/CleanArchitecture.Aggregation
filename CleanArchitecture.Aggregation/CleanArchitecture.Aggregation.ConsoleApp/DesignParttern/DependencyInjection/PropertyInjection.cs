namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.DependencyInjection
{
    /// <summary>
    /// Property Injection là cách tiếp cận mà các dependency được tiêm vào thông qua các thuộc tính public của lớp
    /// Tuy nhiên, trong .NET Core, Property Injection không được khuyến khích sử dụng do khả năng quản lý lifecycle và testing không tốt như Constructor Injection
    /// </summary>
    public interface IPropertyInjectionService
    {
        void PrintMessage();
    }

    public class PropertyInjectionService : IPropertyInjectionService
    {
        public void PrintMessage()
        {
            Console.WriteLine("Hello from PropertyInjectionService");
        }
    }

    public class PropertyInjection
    {
        public IPropertyInjectionService Service { get; set; }

        public void Run()
        {
            Service.PrintMessage();
        }
    }

    public static class RunPropertyInjection
    {
        public static void Run()
        {
            var propertyInjection = new PropertyInjection();
            propertyInjection.Service = new PropertyInjectionService();
            propertyInjection.Run();
        }
    }
}
