namespace Library.Infrastructure.Data;

public class MongoDbSettings
{
   public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string BooksCollectionName { get; set; } = "books";
    public string AuthorsCollectionName { get; set; } = "authors";
    public string CategoriesCollectionName { get; set; } = "categories";
    public string PublishersCollectionName { get; set; } = "publishers";
    public string CountriesCollectionName { get; set; } = "countries";
}