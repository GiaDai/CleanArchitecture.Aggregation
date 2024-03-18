using CleanArchitecture.Aggregation.MongoApi.Models;

namespace CleanArchitecture.Aggregation.MongoApi.Interfaces
{
    public interface ICourseService
    {
        Task<Course?> Create(Course course);
        Task<Course?> GetById(string id);
    }
}
