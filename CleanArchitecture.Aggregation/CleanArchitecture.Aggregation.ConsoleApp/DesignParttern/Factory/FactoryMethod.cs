namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Factory
{
    /// <summary>
    /// Factory Method Pattern mở rộng Simple Factory Pattern bằng cách đưa việc tạo đối tượng vào trong các lớp con (subclasses)
    /// Mỗi lớp con có một Factory Method để tạo ra đối tượng của chính nó hoặc của các lớp con khác trong cùng một hệ thống.
    /// Factory Method Pattern giúp chúng ta áp dụng nguyên tắc đa hình (polymorphism) trong việc tạo đối tượng, cho phép mở rộng và thay đổi logic tạo đối tượng mà không cần sửa đổi client code.
    /// </summary>

    public class VeggiePizza : Pizza
    {
        public override void Prepare()
        {
            Console.WriteLine("Preparing Veggie Pizza");
        }

        public override void Bake()
        {
            Console.WriteLine("Baking Veggie Pizza");
        }
    }

    public abstract class PizzaStore
    {
        public Pizza OrderPizza()
        {
            Pizza pizza = CreatePizza();

            pizza.Prepare();
            pizza.Bake();

            return pizza;
        }

        protected abstract Pizza CreatePizza();
    }

    public class NewYorkPizzaStore : PizzaStore
    {
        protected override Pizza CreatePizza()
        {
            return new CheesePizza();
        }
    }

    public class ChicagoPizzaStore : PizzaStore
    {
        protected override Pizza CreatePizza()
        {
            return new PepperoniPizza();
        }
    }

    public class CaliforniaPizzaStore : PizzaStore
    {
        protected override Pizza CreatePizza()
        {
            return new VeggiePizza();
        }
    }

    public class RunFactoryMethod
    {
        public static void Run()
        {
            PizzaStore nyStore = new NewYorkPizzaStore();
            nyStore.OrderPizza();

            PizzaStore chicagoStore = new ChicagoPizzaStore();
            chicagoStore.OrderPizza();

            PizzaStore californiaStore = new CaliforniaPizzaStore();
            californiaStore.OrderPizza();
        }
    }   
}
