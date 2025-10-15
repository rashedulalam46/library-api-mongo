using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Entities;

public class Countries
{
    // MongoDB _id field
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [Key]
    public int country_id { get; set; }
    public string? country_name { get; set; }
    public string? nationality { get; set; }
    public string? calling_code { get; set; }
}