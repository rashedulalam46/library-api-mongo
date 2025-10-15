using Library.Application.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }


        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetAll()
        {
            var categories = await _service.GetCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categories>> GetById(int id)
        {
            var category = await _service.GetCategoryAsync(id);
            if (category is null) return NotFound();
            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<Categories>> Create([FromBody] Categories category)
        {
            var createdCategory = await _service.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.category_id }, createdCategory);
        }

        // PUT: api/categories/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Categories>> Update(int id, [FromBody] Categories category)
        {
            if (id != category.category_id)
                return BadRequest("Category ID mismatch");

            var updatedCategory = await _service.UpdateCategoryAsync(category);
            if (updatedCategory is null) return NotFound();

            return Ok(updatedCategory);
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteCategoryAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}