using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;

public class PublishersRepository : IPublishersRepository
{
    private readonly IMongoCollection<Publishers> _publishersCollection;

    public PublishersRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _publishersCollection = database.GetCollection<Publishers>(mongoSettings.Value.PublishersCollectionName);
    }

    // Get all publishers
    public async Task<IEnumerable<Publishers>> GetAllAsync()
    {
        var publishers = await _publishersCollection.Find(_ => true).ToListAsync();
        return publishers.OrderBy(p => p.publisher_name);
    }

    // Get a publisher by ID
    public async Task<Publishers?> GetByIdAsync(int id)
    {
        return await _publishersCollection
            .Find(p => p.publisher_id == id)
            .FirstOrDefaultAsync();
    }

    // Add a new publisher
    public async Task<Publishers> AddAsync(Publishers publisher)
    {
        await _publishersCollection.InsertOneAsync(publisher);
        return publisher;
    }

    // Update an existing publisher
    public async Task<Publishers?> UpdateAsync(Publishers publisher)
    {
        var existing = await _publishersCollection
            .Find(p => p.publisher_id == publisher.publisher_id)
            .FirstOrDefaultAsync();

        if (existing == null) return null;

        await _publishersCollection.ReplaceOneAsync(p => p.publisher_id == publisher.publisher_id, publisher);
        return publisher;
    }

    // Delete a publisher
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _publishersCollection.DeleteOneAsync(p => p.publisher_id == id);
        return result.DeletedCount > 0;
    }

    // Check if publisher exists
    public async Task<bool> ExistsByPublisherIdAsync(int publisherId)
    {
        return await _publishersCollection
            .Find(p => p.publisher_id == publisherId)
            .AnyAsync();
    }
}
