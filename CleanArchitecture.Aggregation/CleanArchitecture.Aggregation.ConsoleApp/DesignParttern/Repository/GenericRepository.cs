namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.Repository
{
    /// <summary>
    /// Generic Repository Pattern là một cách triển khai linh hoạt và tổng quát của Repository Pattern trong .NET Core.
    /// Trong Generic Repository Pattern, bạn tạo một lớp Repository chung và sử dụng các generics để làm cho nó có thể hoạt động với bất kỳ loại đối tượng nào.
    /// Các phương thức cơ bản như GetById, GetAll, Add, Update, Delete được định nghĩa trong Generic Repository và có thể tái sử dụng cho các đối tượng khác nhau.
    /// </summary>

    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        T Update(int id, T entity);
        void Delete(int id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly List<T> _entities = new List<T>();
        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Delete(int id)
        {
            _entities.Remove(_entities.FirstOrDefault(e => e.GetHashCode() == id));

        }

        public IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public T GetById(int id)
        {
            return _entities.FirstOrDefault(e => e.GetHashCode() == id);
        }

        public T Update(int id, T entity)
        {
           var _entity = _entities.FirstOrDefault(e => e.GetHashCode() == id);
            if (_entity != null)
            {
                _entity = entity;
            }
            return _entity;
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RunGenericRepository
    {
        public void Test()
        {
            IGenericRepository<Student> studentRepository = new GenericRepository<Student>();
            studentRepository.Add(new Student { Id = 1, Name = "Student 1" });
            studentRepository.Add(new Student { Id = 2, Name = "Student 2" });
            studentRepository.Add(new Student { Id = 3, Name = "Student 3" });
            studentRepository.Add(new Student { Id = 4, Name = "Student 4" });

            var student = studentRepository.GetById(2);
            student.Name = "Student 2 - Updated";
            studentRepository.Update(2, student);
            studentRepository.Delete(3);
            var students = studentRepository.GetAll();
            foreach (var item in students)
            {
                Console.WriteLine($"Id: {item.Id}, Name: {item.Name}");
            }

            IGenericRepository<Teacher> teacherRepository = new GenericRepository<Teacher>();
            teacherRepository.Add(new Teacher { Id = 1, Name = "Teacher 1" });
            teacherRepository.Add(new Teacher { Id = 2, Name = "Teacher 2" });
            teacherRepository.Add(new Teacher { Id = 3, Name = "Teacher 3" });
            teacherRepository.Add(new Teacher { Id = 4, Name = "Teacher 4" });

            var teacher = teacherRepository.GetById(2);
            teacher.Name = "Teacher 2 - Updated";
            teacherRepository.Update(2, teacher);
            teacherRepository.Delete(3);
            var teachers = teacherRepository.GetAll();
            foreach (var item in teachers)
            {
                Console.WriteLine($"Id: {item.Id}, Name: {item.Name}");
            }
        }
    }
}
