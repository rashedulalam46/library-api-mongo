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
        var books = await _booksCollection.Find(_ => true).ToListAsync();
        var authors = await _authorsCollection.Find(_ => true).ToListAsync();
        var categories = await _categoriesCollection.Find(_ => true).ToListAsync();
        var publishers = await _publishersCollection.Find(_ => true).ToListAsync();

        var result = books.Select(b =>
        {
            var author = authors.FirstOrDefault(a => a.author_id == b.author_id);
            var category = categories.FirstOrDefault(c => c.category_id == b.category_id);
            var publisher = publishers.FirstOrDefault(p => p.publisher_id == b.publisher_id);

            return new BookReadDto
            {
                book_id = b.book_id,
                title = b.title,
                description = b.description,
                author_id = b.author_id,
                category_id = b.category_id,
                publisher_id = b.publisher_id,
                author_name = author?.author_name,
                category_name = category?.category_name,
                publisher_name = publisher?.publisher_name,
                isbn = b.isbn,
                price = b.price,
                publish_date = b.publish_date,
                active = b.active
            };
        })
        .OrderBy(b => b.title);

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
