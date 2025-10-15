using Library.Application.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _service;

        public AuthorController(AuthorService service)
        {
            _service = service;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Authors>>> GetAll()
        {
            var authors = await _service.GetAuthorsAsync();
            return Ok(authors);
        }

        // GET: api/authors/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Authors>> GetById(int id)
        {
            var author = await _service.GetAuthorAsync(id);
            if (author is null) return NotFound();
            return Ok(author);
        }

        // POST: api/authors
        [HttpPost]
        public async Task<ActionResult<Authors>> Create([FromBody] Authors author)
        {
            var createdAuthor = await _service.AddAuthorAsync(author);
            return CreatedAtAction(nameof(GetById), new { id = createdAuthor.author_id }, createdAuthor);
        }

        // PUT: api/authors/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Authors>> Update(int id, [FromBody] Authors author)
        {
            if (id != author.author_id)
                return BadRequest("Author ID mismatch");

            var updatedAuthor = await _service.UpdateAuthorAsync(author);
            if (updatedAuthor is null) return NotFound();

            return Ok(updatedAuthor);
        }

        // DELETE: api/authors/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAuthorAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
