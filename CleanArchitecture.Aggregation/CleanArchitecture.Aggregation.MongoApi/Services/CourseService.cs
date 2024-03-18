﻿using CleanArchitecture.Aggregation.MongoApi.Interfaces;
using CleanArchitecture.Aggregation.MongoApi.Models;
using CleanArchitecture.Aggregation.MongoApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CleanArchitecture.Aggregation.MongoApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public CourseService(IOptions<SchoolDatabaseSettings> schoolDatabaseSettings, IMongoClient client)
        {
            var database = client.GetDatabase(schoolDatabaseSettings.Value.DatabaseName);

            _courseCollection = database.GetCollection<Course>(schoolDatabaseSettings.Value.CoursesCollectionName);
        }

        public async Task<Course?> GetById(string id)
        {
            return await _courseCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Course?> Create(Course course)
        {
            course.Id = ObjectId.GenerateNewId().ToString();

            await _courseCollection.InsertOneAsync(course);

            return course;
        }
    }
}
