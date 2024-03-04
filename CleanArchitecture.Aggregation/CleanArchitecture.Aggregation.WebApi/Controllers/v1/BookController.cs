using CleanArchitecture.Aggregation.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Elastic.Clients.Elasticsearch;

using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Controllers.v1
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ElasticsearchClient _client;
        public BookController(ElasticsearchClient client)
        {
            _client = client;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBook(string title)
        {
            var response = await _client.SearchAsync<Book>(s => s
                .Index("book")
                .Query(q => q
                    // where title contains title
                    .Match(m => m.Field(f => f.Title).Query(title))
                )
            );

            if (response.IsValidResponse)
            {
                return Ok(response.Documents);
            }
            return BadRequest();
        }

        // Delete book has title is null
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBook()
        {
            var response = await _client.DeleteByQueryAsync<Book>("book",d => d
                .Query(q => q
                    // where title is empty
                    .Term(t => t.Field(f => f.Title).Value(""))
                )
            );

            if (response.IsValidResponse)
            {
                return Ok(response.Total);
            }
            return BadRequest();
        }

        // Add new book
        [HttpPost("add")]
        public async Task<IActionResult> AddBook(Book book)
        {
            var response = await _client.IndexAsync(book, "book");
            if (response.IsValidResponse)
            {
                return Ok(response.Id);
            }
            return BadRequest();
        }

        // Add bulk books
        [HttpPost("addbulk")]
        public async Task<IActionResult> AddBulkBooks(Book[] books)
        {
            var response = await _client.BulkAsync(b => b
                           .Index("book")
                                          .IndexMany(books)
                                                     );
            if (response.IsValidResponse)
            {
                return Ok(response.Items.Count);
            }
            return BadRequest();
        }

        // Get all books
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _client.SearchAsync<Book>(s => s
                           .Index("book")
                           .Query(q => q
                           .MatchAll()
                           )
                    );
            if (response.IsValidResponse)
            {
                return Ok(response.Documents);
            }
            return BadRequest();
        }

        // Delete all books
        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAllBooks()
        {
            var response = await _client.DeleteByQueryAsync<Book>("book", d => d
                           .Query(q => q
                           .MatchAll())
                        );

            if (response.IsValidResponse)
            {
                return Ok(response.Total);
            }
            return BadRequest();
        }
    }
}
