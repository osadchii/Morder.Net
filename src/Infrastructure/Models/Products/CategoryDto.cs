namespace Infrastructure.Models.Products;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Guid? ParentId { get; set; }

    public bool DeletionMark { get; set; }

    public Guid ExternalId { get; set; }
}