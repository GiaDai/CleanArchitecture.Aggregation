using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Factory
{
    /// <summary>
    /// Simple Factory Pattern là một pattern đơn giản nhưng mạnh mẽ, nơi một lớp Factory chịu trách nhiệm tạo ra các đối tượng cho client dựa trên một tham số hoặc một điều kiện nào đó.
    /// Trong Simple Factory Pattern, client không cần biết cụ thể làm thế nào để tạo ra đối tượng, chỉ cần gọi phương thức Factory để nhận đối tượng mà nó cần.
    /// </summary>
    
    public enum PizzaType
    {
        Cheese,
        Greek,
        Pepperoni
    }

    public abstract class  Pizza
    {
        public abstract void Prepare();
        public abstract void Bake();
    }

    public class CheesePizza : Pizza
    {
        public override void Prepare()
        {
            Console.WriteLine("Prepare Cheese Pizza");
        }

        public override void Bake()
        {
            Console.WriteLine("Bake Cheese Pizza");
        }
    }

    public class GreekPizza : Pizza
    {
        public override void Prepare()
        {
            Console.WriteLine("Prepare Greek Pizza");
        }

        public override void Bake()
        {
            Console.WriteLine("Bake Greek Pizza");
        }
    }

    public class PepperoniPizza : Pizza
    {
        public override void Prepare()
        {
            Console.WriteLine("Prepare Pepperoni Pizza");
        }

        public override void Bake()
        {
            Console.WriteLine("Bake Pepperoni Pizza");
        }
    }

    public class SimplePizzaFactory
    {
        public Pizza CreatePizza(PizzaType type)
        {
            Pizza pizza = null;

            switch (type)
            {
                case PizzaType.Cheese:
                    pizza = new CheesePizza();
                    break;
                case PizzaType.Greek:
                    pizza = new GreekPizza();
                    break;
                case PizzaType.Pepperoni:
                    pizza = new PepperoniPizza();
                    break;
                default:
                    break;
            }

            return pizza;
        }
    }

    public class SimpleFactory
    {
        public void Run()
        {
            SimplePizzaFactory factory = new SimplePizzaFactory();
            Pizza pizza = factory.CreatePizza(PizzaType.Cheese);
            pizza.Prepare();
            pizza.Bake();
        }
    }
}
