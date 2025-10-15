using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly IMongoCollection<Authors> _authorsCollection;

    public AuthorRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _authorsCollection = database.GetCollection<Authors>(mongoSettings.Value.AuthorsCollectionName);
    }

    public async Task<IEnumerable<Authors>> GetAllAsync()
    {
        var authors = await _authorsCollection
            .Find(_ => true)
            .SortBy(a => a.author_name)
            .ToListAsync();

        return authors;
    }

    public async Task<Authors?> GetByIdAsync(int id)
    {
        return await _authorsCollection
            .Find(a => a.author_id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Authors> AddAsync(Authors author)
    {
        await _authorsCollection.InsertOneAsync(author);
        return author;
    }

    public async Task<Authors?> UpdateAsync(Authors author)
    {
        var existing = await _authorsCollection
            .Find(a => a.author_id == author.author_id)
            .FirstOrDefaultAsync();

        if (existing == null)
            return null;

        await _authorsCollection.ReplaceOneAsync(a => a.author_id == author.author_id, author);
        return author;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _authorsCollection.DeleteOneAsync(a => a.author_id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ExistsByAuthorIdAsync(int authorId)
    {
        return await _authorsCollection.Find(a => a.author_id == authorId).AnyAsync();
    }
}
