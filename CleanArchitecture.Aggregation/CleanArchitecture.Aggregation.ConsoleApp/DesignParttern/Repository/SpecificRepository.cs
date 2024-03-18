namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Repository
{
    /// <summary>
    /// Specific Repository Pattern là một cách triển khai Repository Pattern tập trung vào từng loại đối tượng cụ thể trong ứng dụng.
    /// Trong Specific Repository Pattern, bạn tạo ra các lớp Repository riêng biệt cho từng đối tượng trong hệ thống, ví dụ như UserRepository, ProductRepository, OrderRepository, 
    /// và mỗi lớp này sẽ chứa các phương thức liên quan đến đối tượng tương ứng.
    /// Mỗi lớp Repository có thể cung cấp các phương thức tùy chỉnh và tối ưu cho loại đối tượng của nó.
    /// </summary>

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Add(User entity);
        void Update(User entity);
        void Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();
        public void Add(User entity)
        {
            _users.Add(entity);
        }

        public void Delete(int id)
        {
            _users.Remove(_users.FirstOrDefault(e => e.Id == id));
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(e => e.Id == id);
        }

        public void Update(User entity)
        {
            var _entity = _users.FirstOrDefault(e => e.Id == entity.Id);
            if (_entity != null)
            {
                _entity = entity;
            }
        }
    }

    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product entity);
        void Update(Product entity);
        void Delete(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();
        public void Add(Product entity)
        {
            _products.Add(entity);
        }

        public void Delete(int id)
        {
            _products.Remove(_products.FirstOrDefault(e => e.Id == id));
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Product entity)
        {
            var _entity = _products.FirstOrDefault(e => e.Id == entity.Id);
            if (_entity != null)
            {
                _entity = entity;
            }
        }
    }

    public class RunSpecificRepository
    {
        public void Run()
        {
            IUserRepository userRepository = new UserRepository();
            IProductRepository productRepository = new ProductRepository();

            userRepository.Add(new User { Id = 1, Name = "User 1", Email = "alice@example.com" });
            userRepository.Add(new User { Id = 2, Name = "User 2", Email = "alex@example.com" });

            productRepository.Add(new Product { Id = 1, Name = "Product 1", Price = 100 });

            var users = userRepository.GetAll();
            var products = productRepository.GetAll();

            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"- {user.Name} ({user.Email})");
            }

            Console.WriteLine("Products:");

            foreach (var product in products)
            {
                Console.WriteLine($"- {product.Name} ({product.Price})");
            }
        }
    }
}
