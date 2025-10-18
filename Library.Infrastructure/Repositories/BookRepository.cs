using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IMongoCollection<Books> _booksCollection;
    private readonly IMongoCollection<Authors> _authorsCollection;
    private readonly IMongoCollection<Categories> _categoriesCollection;
    private readonly IMongoCollection<Publishers> _publishersCollection;

    public BookRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

        _booksCollection = database.GetCollection<Books>(mongoSettings.Value.BooksCollectionName);
        _authorsCollection = database.GetCollection<Authors>(mongoSettings.Value.AuthorsCollectionName);
        _categoriesCollection = database.GetCollection<Categories>(mongoSettings.Value.CategoriesCollectionName);
        _publishersCollection = database.GetCollection<Publishers>(mongoSettings.Value.PublishersCollectionName);
    }

    // Get all books with joined names
    public async Task<IEnumerable<BookReadDto>> GetAllAsync()
    {
        var pipeline = _booksCollection.Aggregate()
        // Join authors
        .Lookup("authors", "author_id", "author_id", "author")
        // Join categories
        .Lookup("categories", "category_id", "category_id", "category")
        // Join publishers
        .Lookup("publishers", "publisher_id", "publisher_id", "publisher")
        // Unwind joined arrays (preserve nulls)
        .Unwind("author", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
        .Unwind("category", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
        .Unwind("publisher", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
        // Tell Mongo to output as BsonDocument
        .As<BsonDocument>();

        var docs = await pipeline.ToListAsync();

        var result = docs.Select(b => new BookReadDto
        {
            book_id = b.Contains("book_id") ? b["book_id"].AsInt32 : 0,
            title = b.GetValue("title", "").AsString,
            description = b.GetValue("description", "").AsString,
            author_id = b.Contains("author_id") ? b["author_id"].AsInt32 : (int?)null,
            category_id = b.Contains("category_id") ? b["category_id"].AsInt32 : (int?)null,
            publisher_id = b.Contains("publisher_id") ? b["publisher_id"].AsInt32 : (int?)null,
            author_name = b.Contains("author") && b["author"].IsBsonDocument && b["author"].AsBsonDocument.Contains("author_name")
                ? b["author"]["author_name"].AsString
                : null,
            category_name = b.Contains("category") && b["category"].IsBsonDocument && b["category"].AsBsonDocument.Contains("category_name")
                ? b["category"]["category_name"].AsString
                : null,
            publisher_name = b.Contains("publisher") && b["publisher"].IsBsonDocument && b["publisher"].AsBsonDocument.Contains("publisher_name")
                ? b["publisher"]["publisher_name"].AsString
                : null,
            isbn = b.GetValue("isbn", "").AsString,
            price = b.Contains("price") && b["price"].IsNumeric ? b["price"].ToDecimal() : (decimal?)null,
            publish_date = b.Contains("publish_date") && b["publish_date"].IsValidDateTime ? b["publish_date"].ToUniversalTime() : (DateTime?)null,
            active = b.Contains("active") && b["active"].IsBoolean ? b["active"].ToBoolean() : (bool?)null
        });

        return result;
    }

    // Get single book by book_id
    public async Task<Books?> GetByIdAsync(int id)
    {
        return await _booksCollection
            .Find(b => b.book_id == id)
            .FirstOrDefaultAsync();
    }

    // Add new book
    public async Task<Books> AddAsync(Books book)
    {
        await _booksCollection.InsertOneAsync(book);
        return book;
    }

    // Update existing book
    public async Task<Books?> UpdateAsync(Books book)
    {
        var existing = await _booksCollection
            .Find(b => b.book_id == book.book_id)
            .FirstOrDefaultAsync();

        if (existing == null)
            return null;

        await _booksCollection.ReplaceOneAsync(b => b.book_id == book.book_id, book);
        return book;
    }

    // Delete book
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _booksCollection.DeleteOneAsync(b => b.book_id == id);
        return result.DeletedCount > 0;
    }

    // Check existence by book_id
    public async Task<bool> ExistsByBookIdAsync(int bookId)
    {
        return await _booksCollection
            .Find(b => b.book_id == bookId)
            .AnyAsync();
    }
}
