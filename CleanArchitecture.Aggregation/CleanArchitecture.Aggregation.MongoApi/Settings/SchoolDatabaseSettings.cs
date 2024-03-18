﻿namespace CleanArchitecture.Aggregation.MongoApi.Settings
{
    public class SchoolDatabaseSettings
    {
        public string StudentsCollectionName { get; set; } = null!;

        public string CoursesCollectionName { get; set; } = null!;

        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}
