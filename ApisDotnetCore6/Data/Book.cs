using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApisDotnetCore6.Data;

[Table(name: "book")]
public class Book
{
    [Key] 
    public int Id { get; set; }

    [MaxLength(100)] 
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Range((double)Decimal.One, (double)Decimal.MaxValue)]
    public Decimal Price { get; set; }

    [Range(0, 100)] 
    public int Quantity { get; set; }
    
}