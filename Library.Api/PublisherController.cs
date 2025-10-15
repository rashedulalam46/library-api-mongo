using Library.Application.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class PublisherController : ControllerBase
    {
        private readonly PublishersService _service;

        public PublisherController(PublishersService service)
        {
            _service = service;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publishers>>> GetAll()
        {
            var publishers = await _service.GetPublishersAsync();
            return Ok(publishers);
        }

        // GET: api/publishers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Publishers>> GetById(int id)
        {
            var publisher = await _service.GetPublisherAsync(id);
            if (publisher is null) return NotFound();
            return Ok(publisher);
        }

        // POST: api/publishers
        [HttpPost]
        public async Task<ActionResult<Publishers>> Create([FromBody] Publishers publisher)
        {
            var createdPublisher = await _service.AddPublisherAsync(publisher);
            return CreatedAtAction(nameof(GetById), new { id = createdPublisher.publisher_id }, createdPublisher);
        }

        // PUT: api/publishers/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Publishers>> Update(int id, [FromBody] Publishers publisher)
        {
            if (id != publisher.publisher_id)
                return BadRequest("Publisher ID mismatch");

            var updatedPublisher = await _service.UpdatePublisherAsync(publisher);
            if (updatedPublisher is null) return NotFound();

            return Ok(updatedPublisher);
        }

        // DELETE: api/publishers/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeletePublisherAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
