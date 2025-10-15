using Library.Application.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetAll()
        {
            var books = await _service.GetBooksAsync();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Books>> GetById(int id)
        {
            var book = await _service.GetBookAsync(id);
            if (book is null) return NotFound();
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Books>> Create([FromBody] Books book)
        {
            var createdBook = await _service.AddBookAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.book_id }, createdBook);
        }

        // PUT: api/books/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Books>> Update(int id, [FromBody] Books book)
        {
            if (id != book.book_id)
                return BadRequest("Book ID mismatch");

            var updatedBook = await _service.UpdateBookAsync(book);
            if (updatedBook is null) return NotFound();

            return Ok(updatedBook);
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteBookAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}


// GET /api/books → list all books
// GET /api/books/{id} → get one book
// POST /api/books → create new book
// PUT /api/books/{id} → update book
// DELETE /api/books/{id} → remove book

// Uses standard REST conventions:
// 200 OK for success
// 201 Created with CreatedAtAction after POST
// 404 Not Found if resource doesn’t exist
// 204 No Content after DELETE