using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api;

[ApiController]
[Route("api/[controller]")]
public class DropdownController : ControllerBase
{
    private readonly DropdownService _dropdownService;

    public DropdownController(DropdownService dropdownService)
    {
        _dropdownService = dropdownService;
    }

    // GET: api/dropdown/countries
    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _dropdownService.GetCountryDropdownAsync();
        return Ok(countries);
    }
    
    // GET: api/dropdown/authors
    [HttpGet("authors")]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _dropdownService.GetAuthorDropdownAsync();
        return Ok(authors);
    }

    // GET: api/dropdown/publishers
    [HttpGet("publishers")]
    public async Task<IActionResult> GetPublishers()
    {
        var publishers = await _dropdownService.GetPublisherDropdownAsync();
        return Ok(publishers);
    }

    // GET: api/dropdown/categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _dropdownService.GetCategoryDropdownAsync();
        return Ok(categories);
    }
}