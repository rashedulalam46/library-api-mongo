using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;


public class DropdownRepository : IDropdownRepository
{
    private readonly IMongoCollection<Countries> _countriesCollection;
    private readonly IMongoCollection<Authors> _authorsCollection;
    private readonly IMongoCollection<Publishers> _publishersCollection;
    private readonly IMongoCollection<Categories> _categoriesCollection;

    public DropdownRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

        _countriesCollection = database.GetCollection<Countries>(mongoSettings.Value.CountriesCollectionName);
        _authorsCollection = database.GetCollection<Authors>(mongoSettings.Value.AuthorsCollectionName);
        _publishersCollection = database.GetCollection<Publishers>(mongoSettings.Value.PublishersCollectionName);
        _categoriesCollection = database.GetCollection<Categories>(mongoSettings.Value.CategoriesCollectionName);
    }

    public async Task<IEnumerable<DropdownItem>> GetCountryDropdownAsync()
    {
        var countries = await _countriesCollection.Find(_ => true).ToListAsync();
        return countries
            .Select(c => new DropdownItem
            {
                Value = c.country_id.ToString(),
                Text = c.country_name
            })
            .OrderBy(c => c.Text);
    }

    public async Task<IEnumerable<DropdownItem>> GetAuthorDropdownAsync()
    {
        var authors = await _authorsCollection.Find(_ => true).ToListAsync();
        return authors
            .Select(a => new DropdownItem
            {
                Value = a.author_id.ToString(),
                Text = a.author_name
            })
            .OrderBy(a => a.Text);
    }

    public async Task<IEnumerable<DropdownItem>> GetPublisherDropdownAsync()
    {
        var publishers = await _publishersCollection.Find(_ => true).ToListAsync();
        return publishers
            .Select(p => new DropdownItem
            {
                Value = p.publisher_id.ToString(),
                Text = p.publisher_name
            })
            .OrderBy(p => p.Text);
    }

    public async Task<IEnumerable<DropdownItem>> GetCategoryDropdownAsync()
    {
        var categories = await _categoriesCollection.Find(_ => true).ToListAsync();
        return categories
            .Select(c => new DropdownItem
            {
                Value = c.category_id.ToString(),
                Text = c.category_name
            })
            .OrderBy(c => c.Text);
    }
}
