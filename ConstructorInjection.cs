
using System;
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
namespace CleanArchitecture.Aggregation.ConsoleApp.DesignPatterns.DependencyInjection.ConstructorInjection
public class  RunContructorInjection
{
    private readonly IConstructorInjectionService constructorInjectionService;

    static void Run()
    {
        var service = new ConstructorInjectionService();
        var constructorInjection = new ConstructorInjection(service);
        constructorInjection.Run();
    }
}