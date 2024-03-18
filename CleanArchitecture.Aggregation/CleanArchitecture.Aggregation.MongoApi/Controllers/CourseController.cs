using CleanArchitecture.Aggregation.MongoApi.Interfaces;
using CleanArchitecture.Aggregation.MongoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Aggregation.MongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _CourseService;

        public CourseController(ICourseService service)
        {
            _CourseService = service;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Course>> GetById(string id)
        {
            var course = await _CourseService.GetById(id);

            return course is null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            var createdCourse = await _CourseService.Create(course);

            return CreatedAtAction(nameof(GetById),
                new { id = createdCourse!.Id },
                createdCourse);

            return createdCourse is null
                ? throw new Exception("Course creation failed")
                : CreatedAtAction(nameof(GetById),
                new { id = createdCourse.Id }, createdCourse);
        }
    }
}
