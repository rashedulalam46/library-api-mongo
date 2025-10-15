using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Categories> _categoriesCollection;

    public CategoryRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _categoriesCollection = database.GetCollection<Categories>(mongoSettings.Value.CategoriesCollectionName);
    }

    public async Task<IEnumerable<Categories>> GetAllAsync()
    {
        var categories = await _categoriesCollection.Find(_ => true).ToListAsync();
        return categories.OrderBy(c => c.category_name);
    }

    public async Task<Categories?> GetByIdAsync(int id)
    {
        return await _categoriesCollection
            .Find(c => c.category_id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Categories> AddAsync(Categories category)
    {
        await _categoriesCollection.InsertOneAsync(category);
        return category;
    }

    public async Task<Categories?> UpdateAsync(Categories category)
    {
        var existing = await _categoriesCollection
            .Find(c => c.category_id == category.category_id)
            .FirstOrDefaultAsync();

        if (existing == null)
            return null;

        await _categoriesCollection.ReplaceOneAsync(c => c.category_id == category.category_id, category);
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _categoriesCollection.DeleteOneAsync(c => c.category_id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsByCategoryIdAsync(int categoryId)
    {
        return await _categoriesCollection
            .Find(c => c.category_id == categoryId)
            .AnyAsync();
    }
}
